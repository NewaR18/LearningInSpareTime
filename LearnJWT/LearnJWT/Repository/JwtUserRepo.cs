using LearnJWT.AppDbContext;
using LearnJWT.Model;

namespace LearnJWT.Repository
{
    public class JwtUserRepo : IJwtUser
    {
        private readonly ApplicationDbContext _context;
        public JwtUserRepo(ApplicationDbContext context) {
            _context = context;
        }
        public bool AuthorizeGivenUser(JwtUser user)
        {
            JwtUser user2 =_context.JwtUsers.Where(val => val.Username == user.Username && val.Password == user.Password).FirstOrDefault();
            if (user2 != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
