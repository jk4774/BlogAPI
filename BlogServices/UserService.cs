using System;
using BlogEntities;
using BlogContext;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
// using Microsoft.AspNetCore.Authentication;

namespace BlogServices
{
    public class UserService
    {
        private readonly int SaltSize = 9;
        private readonly Blog _blog;
        public UserService(Blog blog)
        {
            _blog = blog;
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

        public bool Auth(User user)
        {
            var userDb = _blog.Users.FirstOrDefault(x=>x.Email.Equals(user.Email, StringComparison.InvariantCultureIgnoreCase));

            if (userDb == null)
                return false;

            var hashedPassword = user.Password.ToString();

            if (!userDb.Password.Equals(hashedPassword)) 
                return false;
            return true;
        }
    }
}
