using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnFileHandling.Models
{
    public class FileContents
    {
        [Key]
        public int FileId { get; set; }
        public string FileName { get; set; } = "";
        public string FilePath { get; set; } = "";
        [ForeignKey("Users")]
        public int UsersId { get; set; }
    }
}
