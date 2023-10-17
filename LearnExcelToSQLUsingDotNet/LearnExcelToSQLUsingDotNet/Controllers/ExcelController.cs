using LearnExcelToSQLUsingDotNet.Models;
using LearnExcelToSQLUsingDotNet.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NPOI.SS.UserModel;
using NPOI.Util.Collections;
using NPOI.XWPF.UserModel;
using OfficeOpenXml;
using System.Reflection;

namespace LearnExcelToSQLUsingDotNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExcelController : Controller
    {
        private readonly IExcelRepo _excelRepo;
        public ExcelController(IExcelRepo excelRepo) 
        {
            _excelRepo = excelRepo;
        }
        [HttpPost("ReadExcel",Name = "Read Excel")]
        public async Task<IActionResult> insertIntoSQL()
        {
            List<TransactionData> _transactiondatas=new List<TransactionData>();
            List<string> cellValues = new List<string>();
            var formCollection = await Request.ReadFormAsync();
            foreach (var file in formCollection.Files)
            {
                using (var stream = file.OpenReadStream())
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        int startRow = 2;
                        int endRow = worksheet.Dimension.End.Row;
                        List<ColumnDefinition> _columns = _excelRepo.GetColumnDefinitions().ToList();
                        PropertyInfo[] properties = typeof(TransactionData).GetProperties();
                        for (int row = startRow; row <= endRow; row++)
                        {
                            TransactionData transactionData = new TransactionData();
                            for (int columnIndex = 0; columnIndex < _columns.Count; columnIndex++)
                            {
                                PropertyInfo property = properties.FirstOrDefault(p => p.Name == _columns[columnIndex].ColumnName);
                                if (property != null)
                                {
                                    object cellValue = worksheet.Cells[row,_columns[columnIndex].ColumnId].Value;
                                    if (property.PropertyType==typeof(string))
                                    {
                                        cellValue = cellValue==null ? "": cellValue;
                                    }
                                    property.SetValue(transactionData, Convert.ChangeType(cellValue, property.PropertyType));
                                }
                            }
                            if(transactionData.TRAN_ID != 0)
                            {
                                _transactiondatas.Add(transactionData);
                                _excelRepo.AddTransactionData(transactionData);
                            } 
                        }
                    }
                }
            }
            return Ok(_transactiondatas);
        }
        [HttpGet("InsertIntoExcel", Name = "Insert Into Excel")]
        public async Task<IActionResult> insertIntoExcel()
        {
            int countForAmount=0, countForTrace = 0;
            ExcelWorksheet worksheet;
            ExcelPackage package=new ExcelPackage();
            List<TransactionData> _transactionDatas = _excelRepo.GetTransactionData().OrderByDescending(option=>option.TRAN_ID).ToList();
            var _worksheet = package.Workbook.Worksheets.Add("TransactionDataExcelPracticeAgain");
            PropertyInfo[] properties = typeof(TransactionData).GetProperties();
            for(int i=1; i<=properties.Length; i++)
            {
                if(properties[i - 1].Name == "AMOUNT")
                {
                    countForAmount = i;
                }
                if (properties[i - 1].Name == "TRACE")
                {
                    countForTrace = i;
                }
                _worksheet.Cells[1, i].Value = properties[i-1].Name;
            }
            for (int i = 0;i<_transactionDatas.Count;i++)
            {
                for (int j = 1; j <= properties.Length; j++)
                {
                    var property = properties[j-1];
                    var valueOfProperty = property.GetValue(_transactionDatas[i]);
                    _worksheet.Cells[i + 2, j].Value = valueOfProperty;
                }
            }
            string sumQuery = string.Format("SUBTOTAL(9,{0})",new ExcelAddress(2, countForAmount, _transactionDatas.Count + 1, countForAmount).Address);
            _worksheet.Cells[_transactionDatas.Count + 2, countForAmount].Style.Numberformat.Format = "#,##0.00";
            _worksheet.Cells[_transactionDatas.Count + 2, countForAmount].Formula = sumQuery;
            _worksheet.Cells[2, countForTrace, _transactionDatas.Count + 1,countForTrace].AutoFilter = true;
            _worksheet.Cells.AutoFitColumns(0);
            _worksheet.Column(countForAmount).Width = 15;
            _worksheet.Cells[_transactionDatas.Count + 2, countForAmount, _transactionDatas.Count + 2, countForAmount+1].Merge = true;
            using (MemoryStream stream = new MemoryStream())
            {
                package.SaveAs(stream);
                stream.Position = 0;
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TransactionDataExcelPracticeAgain.xlsx");
            }
        }
        [HttpPost("ReadExcel2", Name = "Read Excel2")]
        public async Task<IActionResult> insertIntoSQLMergedRows()
        {
            List<PriceCalculation> priceCalculationList = new List<PriceCalculation>();
            List<string> cellValues = new List<string>();
            var formCollection = await Request.ReadFormAsync();
            foreach (var file in formCollection.Files)
            {
                using (var stream = file.OpenReadStream())
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        int startRow = 2;
                        int endRow = worksheet.Dimension.End.Row;
                        object mergedValue=null;
                        int mergedRange = 0;
                        int mergedColumn = 0;
                        PropertyInfo[] properties = typeof(PriceCalculation).GetProperties();
                        for (int row = startRow; row <= endRow; row++)
                        {
                            PriceCalculation priceCalculation = new PriceCalculation();
                            for (int columnIndex = 0; columnIndex < properties.Length; columnIndex++)
                            {
                                PropertyInfo property = properties.FirstOrDefault(p => p.Name == properties[columnIndex].Name);
                                if (property != null)
                                {
                                    if(worksheet.Cells[row, columnIndex + 1].Merge && worksheet.Cells[row, columnIndex + 1].Value != null)
                                    {
                                        mergedValue = worksheet.Cells[row, columnIndex + 1].Value;
                                        string mergedRangeAddress = FindMergedRangeAddress(worksheet, worksheet.Cells[row, columnIndex + 1]);
                                        mergedRange = worksheet.Cells[mergedRangeAddress].Rows;
                                        mergedColumn = columnIndex + 1;
                                    }
                                    object cellValue = worksheet.Cells[row, columnIndex+1].Value;
                                    if (mergedRange != 1)
                                    {
                                        if(cellValue == null && mergedColumn== columnIndex + 1)
                                        {
                                            cellValue = mergedValue;
                                            mergedRange--;
                                        }
                                    }
                                    else
                                    {
                                        mergedValue = null;
                                    }
                                    if (property.PropertyType == typeof(string))
                                    {
                                        cellValue = cellValue == null ? "" : cellValue;
                                    }
                                    if (property.PropertyType == typeof(decimal))
                                    {
                                        cellValue = cellValue == null ? 0.00 : cellValue;
                                    }
                                    property.SetValue(priceCalculation, Convert.ChangeType(cellValue, property.PropertyType));
                                }
                            }
                            if (priceCalculation.Name != "")
                            {
                                priceCalculationList.Add(priceCalculation);
                                _excelRepo.AddPriceCalculation(priceCalculation);
                            }
                        }
                    }
                }
            }
            return Ok(priceCalculationList);
        }
        static string FindMergedRangeAddress(ExcelWorksheet worksheet, ExcelRangeBase cell)
        {
            var mergedCells = worksheet.MergedCells;
            foreach (var mergedAddress in mergedCells)
            {
                var mergedRange = worksheet.Cells[mergedAddress];
                if (mergedRange.Any(c => c.Address == cell.Address))
                {
                    return mergedAddress;
                }
            }
            return null; 
        }
    }
}
