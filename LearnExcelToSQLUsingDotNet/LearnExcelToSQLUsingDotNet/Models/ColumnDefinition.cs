using System.ComponentModel.DataAnnotations;

namespace LearnExcelToSQLUsingDotNet.Models
{
    public class ColumnDefinition
    {
        [Key]
        public int ColumnId { get; set; }
        public string ColumnName { get; set; }
    }
}
