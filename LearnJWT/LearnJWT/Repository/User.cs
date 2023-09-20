using LearnJWT.AppDbContext;
using LearnJWT.Model;
using System.Net;

namespace LearnJWT.Repository
{
    public class User : IUser
    {
        private readonly ApplicationDbContext _context;
        public User(ApplicationDbContext context) {
            _context = context;
        }
        public void DeleteUser(Users user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public Users Find(int id)
        {
            Users user= _context.Users.Find(id);
            return user;
        }

        public IEnumerable<Users> GetUsers()
        {
            IEnumerable<Users> users = _context.Users;
            return users;
        }

        public void InsertUser(Users user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(Users user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
