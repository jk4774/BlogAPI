using System;
using BlogEntities;
using BlogContext;
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
    }
}
