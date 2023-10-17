using LearnExcelToSQLUsingDotNet.DatabaseContextOptions;
using LearnExcelToSQLUsingDotNet.Models;

namespace LearnExcelToSQLUsingDotNet.Repository
{
    public class ExcelRepo : IExcelRepo
    {
        private readonly ApplicationDbContext _context;
        public ExcelRepo(ApplicationDbContext context) {
            _context = context;
        }
        public void AddTransactionData(TransactionData transactionData)
        {
            _context.TransactionDatas.Add(transactionData);
            _context.SaveChanges();
        }
        public IEnumerable<ColumnDefinition> GetColumnDefinitions()
        {
            IEnumerable<ColumnDefinition> columnDefinition = _context.ColumnDefinition;
            return columnDefinition;
        }
        public IEnumerable<TransactionData> GetTransactionData()
        {
            IEnumerable<TransactionData> columnDefinition = _context.TransactionDatas;
            return columnDefinition;
        }
        public void AddPriceCalculation(PriceCalculation priceCalculation)
        {
            _context.PriceCalculation.Add(priceCalculation);
            _context.SaveChanges();
        }
    }
}
