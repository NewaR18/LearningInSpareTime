using LearnFileHandling.Database_Context;
using LearnFileHandling.Models;
using LearnFileHandling.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnFileHandling.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public UserRepo(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public void Add(Users user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public UserWithFile GetUserDetails(int id)
        {
            var user = _context.Users.Include(s => s.FileContents).Where(a => a.Id == id).Select(u => new
            {
                u.FirstName,
                u.LastName,
                u.Email,
                FileDetails = u.FileContents.Select(fc => new { fc.FileName, fc.FilePath }).Select(u => new FileContentAttributes
                {
                    FileName = u.FileName,
                    FilePath = u.FilePath
                })
            }).Select(u => new UserWithFile
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                FileDetails = u.FileDetails
            }).FirstOrDefault();
            UserWithFile files = user != null ? user : new UserWithFile();
            return files;
        }
        public List<FileContentAttributes> DeleteUser(int id)
        {
            var result = _context.Users.Include(s => s.FileContents).Where(s => s.Id == id).FirstOrDefault();
            Users user = result != null ? result : new Users();
            List<FileContentAttributes> files = new List<FileContentAttributes>();
            foreach (var file in user.FileContents)
            {
                files.Add(new FileContentAttributes
                {
                    FileName = file.FileName,
                    FilePath = file.FilePath
                });
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            return files;
        }
        public List<UserWithFile> GetUsers()
        {
            string hostedURL = _configuration["HostedServerURL"];
            List<UserWithFile> users = _context.Users.Include(s => s.FileContents).Select(u => new
            {
                u.FirstName,
                u.LastName,
                u.Email,
                FileDetails = u.FileContents.Select(fc => new { fc.FileName, fc.FilePath }).Select(u => new FileContentAttributes
                {
                    FileName = u.FileName,
                    FilePath = hostedURL + u.FilePath
                })
            }).AsEnumerable().Select(u => new UserWithFile
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                FileDetails = u.FileDetails
            }).ToList();
            return users;
        }
        public string GetGUID(int id)
        {
            var result = _context.Users.Where(p => p.Id == id).Select(s => s.GUID).FirstOrDefault();
            string GUID = result != null ? result.ToString() : "";
            return GUID;
        }

        public FileContents GetFilePath(string fileName)
        {
            var result = _context.FileContents
            .Where(p => p.FileName == fileName)
            .Select(s => new
            {
                s.FileId,
                s.FilePath,
                s.FileName
            })
            .Select(s => new FileContents
            {
                FileId = s.FileId,
                FilePath = s.FilePath,
                FileName = s.FileName
            })
            .FirstOrDefault();
            FileContents fileContents = result != null ? result : new FileContents();
            return fileContents;
        }

        public void Update(Users user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            /*var userDetail = _context.Users.Find(user.Id);
            if (userDetail != null)
            {
                userDetail.FirstName = user.FirstName;
                userDetail.LastName = user.LastName;
                userDetail.Email = user.Email;
            }
            _context.SaveChanges();
            foreach (var file in user.FileContents)
            {
                var row = _context.FileContents.Find(user.FileContents);
            }*/
        }

        public int GetFileIdByFilePath(string FilePath)
        {
            var fileId = _context.FileContents.Where (s=>s.FilePath == FilePath).Select(s=>s.FileId).FirstOrDefault();
            return fileId;
        }
        public void DeleteFile(int id)
        {
            var result = _context.FileContents.Where(s => s.FileId == id).FirstOrDefault();
            FileContents fileContent = result != null ? result : new FileContents();
            _context.FileContents.Remove(fileContent);
            _context.SaveChanges();
        }
    }
}
