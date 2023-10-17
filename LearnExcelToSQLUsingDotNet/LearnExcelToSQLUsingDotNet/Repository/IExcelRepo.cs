using LearnExcelToSQLUsingDotNet.Models;

namespace LearnExcelToSQLUsingDotNet.Repository
{
    public interface IExcelRepo
    {
        public IEnumerable<ColumnDefinition> GetColumnDefinitions();
        public void AddTransactionData(TransactionData transactionData);
        public void AddPriceCalculation(PriceCalculation priceCalculation);
        public IEnumerable<TransactionData> GetTransactionData();
    }
}
