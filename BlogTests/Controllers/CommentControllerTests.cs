using System;
using System.Security.Principal;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using BlogFakes;
using BlogServices;
using BlogContext;
using BlogData.Entities;
using FakeItEasy;
using NUnit.Framework;
using BlogMvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogTests.Controllers
{
    public class CommentControllerTests
    {
        private readonly User user = new User { Id = 1, Email = "q@q.com", Password = "lalalala1!" };
        private readonly Article article = new Article { Id = 1, UserId = 1, Author = "q@q.com", Content = "test-article-content", Date = DateTime.Now, Title = "test-article-title" };
        private readonly Comment comment = new Comment { Id = 1, ArticleId = 1, UserId = 1, Date = DateTime.Now, Content = "test-comment-content", Author = "q@q.com" };
        
        private List<User> Users => new List<User> { user };
        private List<Article> Articles => new List<Article> { article };
        private List<Comment> Comments => new List<Comment> { comment };

        private IBlogDbContext fakeBlog;
        private CommentService fakeCommentService;
        private IHttpContextAccessor httpContextAccessor;
        private DefaultHttpContext context;
        private GenericIdentity fakeIdentity;
        private GenericPrincipal principal;

        private CommentController commentController; 

        [SetUp]
        public void Setup()
        {
            var fakeUserDbSet = new FakeUserDbSet() { data = new ObservableCollection<User>(Users) };
            var fakeArticleDbSet = new FakeArticleDbSet() { data = new ObservableCollection<Article>(Articles) };
            var fakeCommentDbSet = new FakeCommentDbSet() { data = new ObservableCollection<Comment>(Comments) };

            fakeBlog = A.Fake<IBlogDbContext>();
            fakeCommentService = A.Fake<CommentService>();
            httpContextAccessor = A.Fake<IHttpContextAccessor>();
            context = new DefaultHttpContext();
            fakeIdentity = A.Fake<GenericIdentity>();
            principal = A.Fake<GenericPrincipal>();

            A.CallTo(() => fakeBlog.Users).Returns(fakeUserDbSet);
            A.CallTo(() => fakeBlog.Articles).Returns(fakeArticleDbSet);
            A.CallTo(() => fakeBlog.Comments).Returns(fakeCommentDbSet);

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);

            A.CallTo(() => fakeCommentService.AnyArticleById(fakeBlog, A<int>.That.Matches(x => x == 1))).Returns(true);
            A.CallTo(() => fakeIdentity.Name).Returns("1");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            commentController = new CommentController(fakeBlog, fakeCommentService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };
        }

        [Test]
        public void GET_Add_ArticleIdDoesNotExist_ShouldReturnRedirectToAction()
        {
            var result = commentController.Add(2) as RedirectToActionResult;

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("GetById", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
        }

        [Test]
        public void GET_Add_ArticleExists_ShouldReturnView()
        {
            var result = commentController.Add(1) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void POST_Add_ArticleWithThisIdDoesNotExist_ShouldReturnNotFound()
        {
            var result = commentController.Add(2, A.Fake<Comment>()) as NotFoundResult;

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void POST_Add_ArticleWithThisIdDoesExist_ShouldReturnRedirectToAction()
        {
            var result = commentController.Add(1, A.Fake<Comment>()) as RedirectToActionResult;

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("GetById", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
        }

        [Test]
        public async Task GET_Update_CommentWithThisIdIsNull_ShouldReturnRedirectToAction()
        {
            var result = await commentController.Update(2) as RedirectToActionResult;

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("GetById", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
        }

        [Test]
        public async Task GET_Update_CommentWithThisIdIsNotMatchingIdentityName_ShouldReturnRedirectToAction()
        {
            A.CallTo(() => fakeIdentity.Name).Returns("4");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            commentController = new CommentController(fakeBlog, fakeCommentService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };
            
            var result = await commentController.Update(1) as RedirectToActionResult;

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("GetById", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
        }

        [Test]
        public async Task GET_Update_CommentIsOk_ShouldReturnView()
        {
            var result = await commentController.Update(1) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task PUT_Update_CommentWithThisIdIsNull_ShouldReturnRedirectToAction()
        {
            var result = await commentController.Update(2, A.Fake<Comment>()) as RedirectToActionResult;

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("GetById", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
        }

        [Test]
        public async Task PUT_Update_CommentWithThisIdIsNotMatchingIdentityName_ShouldReturnRedirectToAction()
        {
            A.CallTo(() => fakeIdentity.Name).Returns("4");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            commentController = new CommentController(fakeBlog, fakeCommentService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };
            
            var result = await commentController.Update(1, A.Fake<Comment>()) as RedirectToActionResult;

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("GetById", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
        }

        [Test]
        public async Task PUT_Update_CommentIsOk_ShouldReturnNoContent()
        {
            var result = await commentController.Update(1, A.Fake<Comment>()) as NoContentResult;

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        
        [Test]
        public async Task DELETE_Delete_CommentWithThisIdIsNull_ShouldReturnRedirectToAction()
        {
            var result = await commentController.Delete(2) as RedirectToActionResult;

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("GetById", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
        }

        [Test]
        public async Task DELETE_Delete_CommentWithThisIdIsNotMatchingIdentityName_ShouldReturnRedirectToAction()
        {
            A.CallTo(() => fakeIdentity.Name).Returns("4");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            commentController = new CommentController(fakeBlog, fakeCommentService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };
            
            var result = await commentController.Delete(1) as RedirectToActionResult;

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("GetById", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
        }

        [Test]
        public async Task DELETE_Delete_CommentIdExist_ShouldReturnNoContent()
        {
            var result = await commentController.Delete(1) as NoContentResult;

            Assert.IsInstanceOf<NoContentResult>(result);
        }
    }
}