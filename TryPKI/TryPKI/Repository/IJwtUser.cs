using LearnJWT.Model;

namespace LearnJWT.Repository
{
    public interface IJwtUser
    {
        public bool AuthorizeGivenUser(JwtUser user);
    }
}
