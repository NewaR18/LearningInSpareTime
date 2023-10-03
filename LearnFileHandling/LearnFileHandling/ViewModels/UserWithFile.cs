namespace LearnFileHandling.ViewModels
{
    public class UserWithFile
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public IEnumerable<FileContentAttributes> FileDetails { get; set; }=new List<FileContentAttributes>();
    }
}
