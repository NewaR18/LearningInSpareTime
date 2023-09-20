using LearnJWT.Model;
using LearnJWT.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnJWT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUser _user;
        public UserController(IUser user) {
            _user = user;
        }
        [HttpGet]
        [Route("[Action]")]
        [Authorize]
        public IActionResult GetUsers()
        {
            IEnumerable<Users> users = _user.GetUsers();
            return Ok(users);
        }
        [HttpPost]
        [Route("[Action]")]
        [Authorize]
        public IActionResult UpdateUser(Users user)
        {
            _user.UpdateUser(user);
            return Ok("Update Success");
        }
        [HttpDelete]
        [Route("[Action]")]
        [Authorize]
        public IActionResult DeleteUser(int id)
        {
            Users user=_user.Find(id);
            _user.DeleteUser(user);
            return Ok("Return Success");
        }
        [HttpPost]
        [Route("[Action]")]
        [Authorize]
        public IActionResult InsertUser(Users user) {
            _user.InsertUser(user);
            return Ok("Inserted Successfully");
        }
        [HttpGet]
        [Route("[Action]")]
        [Authorize]
        public IActionResult Find(int id)
        {
            Users user= _user.Find(id);
            return Ok(user);
        }
    }
}
