using System.ComponentModel.DataAnnotations;

namespace LearnFileHandling.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string GUID { get; set; } = "";
        public virtual List<FileContents> FileContents { get; set; }=new List<FileContents>();
    }
}
