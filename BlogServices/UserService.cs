using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using BlogData.Entities;
using BlogContext;

namespace BlogServices
{
    public class UserService
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;        
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual async Task SignIn(User user)
        {
            var expires = DateTime.UtcNow.AddMinutes(60);

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var userIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });
            var authProperties = new AuthenticationProperties { ExpiresUtc = expires };

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                userPrincipal,
                authProperties);

            _httpContextAccessor.HttpContext.User = userPrincipal;
        }

        public virtual async Task SingOut()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public virtual string Hash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var data = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var stringBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                    stringBuilder.Append(data[i].ToString("x2"));
        
                return stringBuilder.ToString();
            }
        }

        public virtual bool Verify(string password, string hashedPassword)
        {
            using (var sha256 = SHA256.Create()) 
            {
                var hash = Hash(password);
                var stringComparer = StringComparer.OrdinalIgnoreCase;

                return stringComparer.Compare(hash, hashedPassword) == 0;
            }
        }

        public virtual bool IsEmailAvailable(IBlogDbContext blogDbContext, string email)
        {
            return blogDbContext.Users.Any(x => x.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
        }

        public virtual User GetUserByEmail(IBlogDbContext blogDbContext, string email)
        {
            return blogDbContext.Users.FirstOrDefault(x => x.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}