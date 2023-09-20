using System.ComponentModel.DataAnnotations;

namespace LearnJWT.Model
{
    public class JwtUser
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
