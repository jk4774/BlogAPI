using BlogContext;
using BlogMvc.Controllers;
using FakeItEasy;
using NUnit.Framework;

namespace BlogTests
{
    public class ArticleControllerTests
    {
        private Blog _blog;
        private ArticleController _articleController;
        [SetUp]
        public void Setup()
        {
            _blog = A.Fake<Blog>();
            _articleController = A.Fake<ArticleController>();
        }
    }
}