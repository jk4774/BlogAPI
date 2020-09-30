using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlogContext;
using BlogData.Entities;
using BlogFakes;
using BlogMvc.Controllers;
using BlogServices;
using FakeItEasy;
using NUnit.Framework;

namespace BlogTests.Controllers
{
    public class ArticleControllerTests
    {
        private readonly User user = new User { Id = 1, Email = "q@q.com", Password = "lalalala1!" };
        private readonly Article article = new Article { Id = 1, UserId = 1, Author = "q@q.com", Content = "test-article-content", Date = DateTime.Now, Title = "test-article-title" };
        private readonly Comment comment = new Comment { Id = 1, ArticleId = 1, UserId = 1, Date = DateTime.Now, Content = "test-comment-content", Author = "q@q.com" };
        
        private List<User> Users => new List<User> { user };
        private List<Article> Articles => new List<Article> { article };
        private List<Comment> Comments => new List<Comment> { comment };

        private IBlogDbContext fakeBlog;
        private IHttpContextAccessor httpContextAccessor;
        private DefaultHttpContext context;
        private GenericIdentity fakeIdentity;
        private GenericPrincipal principal;

        private ArticleController articleController;
        private ArticleService fakeArticleService;

        [SetUp]
        public void Setup()
        {  
            var fakeUserDbSet = new FakeUserDbSet() { data = new ObservableCollection<User>(Users) };
            var fakeArticleDbSet = new FakeArticleDbSet() { data = new ObservableCollection<Article>(Articles) };
            var fakeCommentDbSet = new FakeCommentDbSet() { data = new ObservableCollection<Comment>(Comments) };

            fakeBlog = A.Fake<IBlogDbContext>();
            httpContextAccessor = A.Fake<IHttpContextAccessor>();
            context = new DefaultHttpContext();
            fakeIdentity = A.Fake<GenericIdentity>();
            principal = A.Fake<GenericPrincipal>();
            fakeArticleService = A.Fake<ArticleService>();

            A.CallTo(() => fakeBlog.Users).Returns(fakeUserDbSet);
            A.CallTo(() => fakeBlog.Articles).Returns(fakeArticleDbSet);
            A.CallTo(() => fakeBlog.Comments).Returns(fakeCommentDbSet);

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);

            A.CallTo(() => fakeArticleService.RemoveArticle(fakeBlog, article)).DoesNothing();

            A.CallTo(() => fakeIdentity.Name).Returns("1");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            articleController = new ArticleController(fakeBlog, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };
        }

        [Test]
        public void GET_Add_GetArticleView_ShouldReturnView()
        {
            var result = articleController.Add() as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void POST_Add_AddValidArticle_ShouldReturnRedirectToAction()
        {
            var article = new Article { Id = 2, UserId = 2, Author = "q2@q.com", Content = "test-article-content2", Date = DateTime.Now, Title = "test-article-title2" };

            var result = articleController.Add(article) as RedirectToActionResult;

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("GetById", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
        }

        [Test]
        public void GET_Update_ArticleDoesNotExistInDb_ShouldReturnNotFound()
        {
            var result = articleController.Update(11) as NotFoundResult;

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void GET_Update_ArticleUserIdIsNotMatchingIdentityName_ShouldReturnNotFound()
        {
            A.CallTo(() => fakeIdentity.Name).Returns("3");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            articleController = new ArticleController(fakeBlog, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = articleController.Update(1) as NotFoundResult;

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void GET_Update_ArticleExistsAndTheUserIdOk_ShouldReturnView()
        {
            var result = articleController.Update(1) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task PUT_Update_ArticleDoesNotExistInDb_ShouldReturnNotFound()
        {
            var result = await articleController.Update(3, A.Fake<Article>()) as NotFoundResult;

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task PUT_Update_ArticleUserIdIsDoesNotEqualUserId_ShouldReturnNotFound()
        {
            A.CallTo(() => fakeIdentity.Name).Returns("3");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            articleController = new ArticleController(fakeBlog, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = await articleController.Update(1, A.Fake<Article>()) as NotFoundResult;

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task PUT_Update_ArticleUserIdIsDoesNotEqualUserId_ShouldReturnNoContent()
        {
            var result = await articleController.Update(1, A.Fake<Article>()) as NoContentResult;

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DELETE_Delete_ArticleDoesNotExist_ShouldReturnNotFound()
        {
            var result = await articleController.Delete(3) as NotFoundResult;

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DELETE_ArticleUserIdIsNotMatchingIdentityName_ShouldReturnNotFound()
        {
            A.CallTo(() => fakeIdentity.Name).Returns("3");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            articleController = new ArticleController(fakeBlog, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = await articleController.Delete(1) as NotFoundResult;

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DELETE_Delete_ArticleIdIsCorrectShouldRemoveTheArticleWithAllTheComments_ShouldReturnNoContent()
        {
            var result = await articleController.Delete(1) as NoContentResult;

            Assert.IsInstanceOf<NoContentResult>(result);
        }        
    }
}