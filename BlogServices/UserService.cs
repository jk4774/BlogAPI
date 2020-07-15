using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using BlogEntities;
using BlogContext;

namespace BlogServices
{
    public class UserService
    {
        private readonly int SaltSize = 9;
        private readonly Blog _blog;
        private readonly IHttpContextAccessor _accessor;

        public UserService(Blog blog, IHttpContextAccessor accessor)
        {
            _blog = blog;
            _accessor = accessor;
        }

        public string HashPassword(string input)
        {
            var rng = new RNGCryptoServiceProvider();
            var buffer = new byte[SaltSize];
            rng.GetBytes(buffer);
            var salt = Convert.ToBase64String(buffer);
            var bytes = Encoding.UTF8.GetBytes(input + salt);
            var sHA256ManagedString = new SHA256Managed();
            var hash = sHA256ManagedString.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public async Task SignIn(User user)
        {
            var userClaims = new List<Claim>();
            userClaims.Add(new Claim(ClaimTypes.Name, user.Id.ToString()));

            var userIdentity = new ClaimsIdentity(userClaims, "CookieAuth");
            var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });
            
            await _accessor.HttpContext.SignInAsync("CookieAuth", userPrincipal);

            _accessor.HttpContext.User = userPrincipal;
        }
    }
}
