using BlogContext;
using BlogMvc.Controllers;
using FakeItEasy;
using NUnit.Framework;

namespace BlogTests
{
    public class CommentControllerTests
    {
        private BlogDbContext _blogDbContext;
        private CommentController _commentController;
        [SetUp]
        public void Setup()
        {
            _blogDbContext = A.Fake<BlogDbContext>();
            _commentController = A.Fake<CommentController>();
            //_commentController = new CommentController(_blogDbContext);
        }
    }
}