using LearnExcelToSQLUsingDotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnExcelToSQLUsingDotNet.DatabaseContextOptions
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<TransactionData> TransactionDatas { get; set; }
        public DbSet<ColumnDefinition> ColumnDefinition { get; set; }
        public DbSet<PriceCalculation> PriceCalculation { get; set; }
    }
}
