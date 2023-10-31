using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using LearnJWT.Model;
using LearnJWT.Repository;

namespace TryPKI.Controllers
{
    public class JwtStateController : Controller
    {
        private readonly IJwtUser _jwtuser;
        private readonly IConfiguration _configuration;
        public JwtStateController(IJwtUser jwtuser, IConfiguration configuration = null)
        {
            _jwtuser = jwtuser;
            _configuration = configuration;
        }
        [HttpPost]
        [Route("[Action]")]
        public string GetToken(JwtUser user)
        {
            bool validate = _jwtuser.AuthorizeGivenUser(user);
            if (validate)
            {
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Id",Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub,user.Username),
                        new Claim(JwtRegisteredClaimNames.Email,user.Password),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(10),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var stringToken = tokenHandler.WriteToken(token);
                return stringToken;
            }
            return "UnAuthorized";
        }
    }
}
