using Blog.API.Controllers;
using Blog.API.Models;
using Blog.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Blog.Tests
{
    public static class Utils
    {
        private static BlogContext GetBlogContext()
        {
            return new BlogContext(new DbContextOptionsBuilder<BlogContext>().UseInMemoryDatabase().Options);
        }

        public static ArticleController GetArticleController()
        {
            return new ArticleController(GetBlogContext());
        }

        public static CommentController GetCommentController()
        {
            return new CommentController(GetBlogContext());
        }

        public static UserController GetUserController()
        {
            var settings = new Settings { SecurityKey = "securityKeyForTesting" };
            IOptions<Settings> options = Options.Create(settings);
            return new UserController(GetBlogContext(), new UserService(options));
        }
    }
}
