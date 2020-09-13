using BlogContext;
using BlogMvc.Controllers;
using FakeItEasy;
using NUnit.Framework;

namespace BlogTests.Controllers
{
    public class ArticleControllerTests
    {
        private BlogDbContext _blogDbContext;
        private ArticleController _articleController;
        [SetUp]
        public void Setup()
        {
            _blogDbContext = A.Fake<BlogDbContext>();
            _articleController = A.Fake<ArticleController>();
        }
    }
}