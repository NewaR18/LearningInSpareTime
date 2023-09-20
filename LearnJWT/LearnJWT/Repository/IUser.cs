using LearnJWT.Model;

namespace LearnJWT.Repository
{
    public interface IUser
    {
        public IEnumerable<Users> GetUsers();
        public void UpdateUser(Users user);
        public void DeleteUser(Users user);
        public void InsertUser(Users user);
        public Users Find(int id);
    }
}
