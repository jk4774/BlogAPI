using Blog.API.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blog.API.Services
{
    public class UserService
    {
        private readonly Settings _settings;

        public UserService(IOptions<Settings> settings)
        {
            _settings = settings.Value;
        }

        public Tuple<User, string> Authenticate(User user)
        //public User Authenticate (User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, user.Id.ToString()) }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(/*_settings.SecurityKey*/"ULTRA_RARE3_PASSWEORAWEF#$%$HU!!@#")), SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            //return new User { Id = user.Id, Name = user.Name, Password = "", Email = user.Email, Token = token };
            return new Tuple<User, string>(new User { Id = user.Id, Name = user.Name, Password = "", Email = user.Email }, tokenHandler.WriteToken(securityToken));
        }
    }
}
