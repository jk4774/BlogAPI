using System;
using BlogEntities;
using BlogContext;
// using Microsoft.AspNetCore.Authentication;

namespace BlogServices
{
    public class CommentService
    {
        private readonly Blog _blog;
        public CommentService(Blog blog)
        {
            _blog = blog;
        }
    }
}
