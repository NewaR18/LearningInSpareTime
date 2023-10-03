using LearnFileHandling.Models;
using LearnFileHandling.Repository;
using LearnFileHandling.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using static System.Net.WebRequestMethods;

namespace LearnFileHandling.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileHandlingController : Controller
    {
        private readonly IUserRepo _repo;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        public FileHandlingController(IUserRepo repo, IWebHostEnvironment hostingEnvironment, IConfiguration configuration=null)
        {
            _repo = repo;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }
        [HttpPost]
        [Route("[Action]")]
        public async Task<IActionResult> InsertDataAndImage()
        {
            int flag = 0;
            Users myUser = new Users();
            List<FileContents> files=new List<FileContents>();
            var formCollection = await Request.ReadFormAsync();
            myUser.FirstName = formCollection["firstName"].ToString();
            myUser.LastName = formCollection["lastName"].ToString();
            myUser.Email = formCollection["email"].ToString();
            myUser.GUID= Guid.NewGuid().ToString();
            foreach(var file in formCollection.Files) {
                if (file.Length > 0)
                {
                    FileContents fileContents = new FileContents();
                    var absolutePath = Directory.GetCurrentDirectory();
                    var fileName= myUser.GUID + "_" + file.FileName;
                    var storeInFolderPath = Path.Combine("wwwroot","Images","UserRegister");
                    var storeInFolderFilePath = Path.Combine(storeInFolderPath, fileName);
                    var storeInDatabasePath= Path.Combine("Images", "UserRegister");
                    var storeInDatabaseFilePath = Path.Combine(storeInDatabasePath, fileName);
                    if (!Directory.Exists(storeInFolderPath))
                    {
                        Directory.CreateDirectory(storeInFolderPath);
                    }
                    fileContents.FileName = fileName;
                    fileContents.FilePath = storeInDatabaseFilePath;
                    if (System.IO.File.Exists(storeInFolderFilePath))
                    {
                        flag = 1;
                    }
                    using (var stream = new FileStream(storeInFolderFilePath, FileMode.Create))
                    {
                        if (flag != 1)
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                    files.Add(fileContents);
                }
            }
            myUser.FileContents= files;
            _repo.Add(myUser);
            if (flag == 1)
            {
                return Ok("Some of the files are duplicated. So, They are not inserted");
            }
            else
            {
                return Ok("Inserted");
            }
        }
        [HttpPost]
        [Route("[Action]")]
        public UserWithFile GrabDataAndImage(int id)
        {
            string ServerURL = _configuration["HostedServerURL"];
            UserWithFile user = _repo.GetUserDetails(id);
            foreach (var file in user.FileDetails) 
            {
                file.FilePath = ServerURL+file.FilePath;
            }
            return user;
        }
        [HttpDelete]
        [Route("[Action]")]
        public IActionResult DeleteUser(int id)
        {
            string Out = "Files with filename";
            List<FileContentAttributes> files=_repo.DeleteUser(id);
            foreach(var file in files)
            {
                var deleteFolderFilePath = Path.Combine("wwwroot",file.FilePath);
                if (System.IO.File.Exists(deleteFolderFilePath))
                {
                    System.IO.File.Delete(deleteFolderFilePath);
                    Out += "'" + file.FileName + "', ";
                }
                else
                {
                    Out += "' '";
                }
            }
            Out+=" has been deleted";
            return Ok(Out);
        }
        [HttpGet]
        [Route("[Action]")]
        public IActionResult GetUsers()
        {
            List<UserWithFile> user = _repo.GetUsers();
            return Ok(user);
        }
        [HttpPost]
        [Route("[Action]")]
        public async Task<IActionResult> UpdateUser()
        {
            int flag = 0;
            Users myUser = new Users();
            List<FileContents> files = new List<FileContents>();
            var formCollection = await Request.ReadFormAsync();
            myUser.Id= Convert.ToInt32(formCollection["id"]);
            myUser.FirstName = formCollection["firstName"].ToString();
            myUser.LastName = formCollection["lastName"].ToString();
            myUser.Email = formCollection["email"].ToString();
            myUser.GUID= _repo.GetGUID(myUser.Id);
            string oldFiles = formCollection["oldFiles"].ToString();
            char separator = ',';
            string[] names = {};
            if (!string.IsNullOrEmpty(oldFiles))
            {
                names = oldFiles.Split(separator);
            }
            //Old Files in String
            foreach (string name in names)
            {
                FileContents fileContents = _repo.GetFilePath(name.Trim());
                string fileName = name.Trim();
                if(fileContents == null)
                {
                    FileContents fileContentsInCaseOfNull=new FileContents();
                    var absolutePath = Directory.GetCurrentDirectory();
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");


                    var storeInFolderPath = Path.Combine("wwwroot", "Images", "UserRegister");
                    var storeInFolderFilePath = Path.Combine(storeInFolderPath, fileName);
                    var storeInDatabasePath = Path.Combine("Images", "UserRegister");
                    var storeInDatabaseFilePath = Path.Combine(storeInDatabasePath, fileName);

                    Directory.CreateDirectory(uploadsFolder);
                    fileContentsInCaseOfNull.FileName = fileName;
                    fileContentsInCaseOfNull.FilePath = Path.Combine(storeInDatabasePath, fileName);
                    files.Add(fileContentsInCaseOfNull);
                }
                else
                {
                    files.Add(fileContents);
                }
            }
            //New Files in File
            foreach (var file in formCollection.Files)
            {
                if (file.Length > 0)
                {
                    FileContents fileContents = new FileContents();
                    var absolutePath = Directory.GetCurrentDirectory();
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                    var fileName = myUser.GUID + "_" + file.FileName;
                    var storeInFolderPath = Path.Combine("wwwroot", "Images", "UserRegister");
                    var storeInFolderFilePath = Path.Combine(storeInFolderPath, fileName);
                    var storeInDatabasePath = Path.Combine("Images", "UserRegister");
                    var storeInDatabaseFilePath = Path.Combine(storeInDatabasePath, fileName);

                    fileContents.FileName = fileName;
                    fileContents.FilePath = storeInDatabaseFilePath;
                    if (System.IO.File.Exists(storeInFolderFilePath))
                    {
                        flag = 1;
                    }
                    if (flag != 1)
                    {
                        using (var stream = new FileStream(storeInFolderFilePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                    files.Add(fileContents);
                }
            }
            UserWithFile user = _repo.GetUserDetails(myUser.Id);
            List<string> FilePathsToBeDeleted = new List<string>();
            foreach(var filePath in user.FileDetails.ToList().Select(s=>s.FilePath))
            {
                if (!files.Select(s => s.FilePath).Contains(filePath))
                {
                    FilePathsToBeDeleted.Add(filePath);
                }
            }
            myUser.FileContents = files;
            _repo.Update(myUser); //For Adding Other Fields and Adding new FileContents
            foreach (var filePath in FilePathsToBeDeleted)
            {
                var deleteFolderFilePath = Path.Combine("wwwroot", filePath);
                if (System.IO.File.Exists(deleteFolderFilePath))
                {
                    int FileId=_repo.GetFileIdByFilePath(filePath);
                    System.IO.File.Delete(deleteFolderFilePath);
                    _repo.DeleteFile(FileId);
                }
            }
            return Ok("Updated");
        }
    }
}
