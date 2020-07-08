using System;
using BlogEntities;
using BlogContext;
using System.Linq;
// using Microsoft.AspNetCore.Authentication;

namespace BlogServices
{
    public class UserService
    {
        private readonly Blog _blog;
        public UserService(Blog blog)
        {
            _blog = blog;
        }

        public bool CheckPassword(User user)
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
