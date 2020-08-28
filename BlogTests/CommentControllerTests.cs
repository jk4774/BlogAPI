using BlogContext;
using BlogMvc.Controllers;
using FakeItEasy;
using NUnit.Framework;

namespace BlogTests
{
    public class CommentControllerTests
    {
        private Blog _blog;
        private CommentController _commentController;
        [SetUp]
        public void Setup()
        {
            _blog = A.Fake<Blog>();
            _commentController = A.Fake<CommentController>();
            _commentController = new CommentController(_blog);
        }
    }
}