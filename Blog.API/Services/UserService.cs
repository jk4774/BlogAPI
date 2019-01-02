using Blog.API.Helpers;
using Blog.API.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public User Authenticate(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.SecurityKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] { new Claim (ClaimTypes.Name, user.Id.ToString()) }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "Issuer",
                Audience = "Audience",
                NotBefore = DateTime.Now
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            }
            ////var authentication = new AuthenticationHeaderValue("Bearer", tokenHandler.WriteToken(token));
            return user;
        }
    }
}
