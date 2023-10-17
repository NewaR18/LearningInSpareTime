using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LearnExcelToSQLUsingDotNet.Models
{
    public class PriceCalculation
    {
        [Key]
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }
}
