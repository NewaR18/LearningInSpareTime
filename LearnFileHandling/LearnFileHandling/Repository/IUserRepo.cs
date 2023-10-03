using LearnFileHandling.Models;
using LearnFileHandling.ViewModels;

namespace LearnFileHandling.Repository
{
    public interface IUserRepo
    {
        public void Add(Users user);
        public UserWithFile GetUserDetails(int id);
        public List<FileContentAttributes> DeleteUser(int id);
        public List<UserWithFile> GetUsers();
        public string GetGUID(int id);
        public FileContents GetFilePath(string FileName);
        public void Update(Users user);
        public int GetFileIdByFilePath(string FilePath);
        public void DeleteFile(int id);
    }
}
