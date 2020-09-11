using System.Threading.Tasks;
using System.Security.Principal;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlogMvc.Controllers;
using BlogData.Entities;
using BlogData.ViewModels;
using BlogServices;
using BlogContext;
using FakeItEasy;
using NUnit.Framework;
using System;
using BlogFakes;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BlogTests
{
    public class UserControllerTests
    {
        private IBlogDbContext fakeBlog;
        private UserService fakeUserService;
        private ArticleService fakeArticleService;
        private IHttpContextAccessor httpContextAccessor;
        private DefaultHttpContext context;
        private GenericIdentity fakeIdentity;
        private GenericPrincipal principal;

        [SetUp]
        public void Setup()
        {
            var user = new User { Id = 1, Email = "q@q.com", Password = "lalalala1!" };
            var article = new Article { Id = 1, UserId = 1, Author = "q@q.com", Content = "test-article-content", Date = DateTime.Now, Title = "test-article-title" };
            var comment = new Comment { Id = 1, ArticleId = 1, UserId = 1, Date = DateTime.Now, Content = "test-comment-content", Author = "q@q.com" };

            var users = new List<User> { user };
            var articles = new List<Article> { article };
            var comments = new List<Comment> { comment };

            var fakeUserDbSet = new FakeUserDbSet() { data = new ObservableCollection<User>(users) };
            var fakeArticleDbSet = new FakeArticleDbSet() { data = new ObservableCollection<Article>(articles) };
            var fakeCommentDbSet = new FakeCommentDbSet() { data = new ObservableCollection<Comment>(comments) };

            fakeBlog = A.Fake<IBlogDbContext>();
            fakeUserService = A.Fake<UserService>();
            fakeArticleService = A.Fake<ArticleService>();
            httpContextAccessor = A.Fake<IHttpContextAccessor>();
            context = new DefaultHttpContext();
            fakeIdentity = A.Fake<GenericIdentity>();
            principal = A.Fake<GenericPrincipal>();

            A.CallTo(() => fakeBlog.Users).Returns(fakeUserDbSet);
            A.CallTo(() => fakeBlog.Articles).Returns(fakeArticleDbSet);
            A.CallTo(() => fakeBlog.Comments).Returns(fakeCommentDbSet);

            A.CallTo(() => fakeArticleService.GetArticleViewModels(fakeBlog)).Returns(new List<ArticleViewModel>
            {
                new ArticleViewModel 
                { 
                    Article = article, 
                    Comments = comments 
                }
            });

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            A.CallTo(() => fakeUserService.SignIn(A.Fake<User>())).DoesNothing();
            A.CallTo(() => fakeUserService.SingOut()).DoesNothing();
            A.CallTo(() => fakeUserService.Verify(
                  A<string>.That.Matches(x => x == "lalalala1!"),
                  A<string>.That.Matches(x => x == "lalalala1!"))).Returns(true);
            A.CallTo(() => fakeUserService.Verify(
                A<string>.That.Matches(x => x != "lalalala1!"),
                A<string>.That.Matches(x => x != "lalalala1!"))).Returns(false);
        }

        [Test]
        public async Task GET_GetById_IncorrectId_ShouldReturnRedirectToAction()
        {
            A.CallTo(() => fakeIdentity.Name).Returns("1");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = await userController.GetById(10) as RedirectToActionResult;

            Assert.AreEqual("GetById", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public async Task GET_GetById_CorrectIdUserDoesNotExistInDb_ShouldReturnRedirectToAction()
        {
            A.CallTo(() => fakeIdentity.Name).Returns("1");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userDbFake = new FakeUserDbSet() { data = new ObservableCollection<User> { } };
            A.CallTo(() => fakeBlog.Users).Returns(userDbFake);

            var userController = new UserController(fakeBlog, fakeUserService, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = await userController.GetById(1) as RedirectToActionResult;

            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public async Task GET_GetById_CorrectIdUserDoesExist_ShouldReturnView()
        {
            A.CallTo(() => fakeIdentity.Name).Returns("1");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = await userController.GetById(1) as ViewResult;

            Assert.IsInstanceOf<UserViewModel>(result.ViewData.Model);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void GET_Login_UserIsAuthenticated_ShouldReturnRedirectToAction()
        {
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(true);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = userController.Login() as RedirectToActionResult;

            Assert.AreEqual("GetById", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void GET_Login_UserIsNotAuthenticated_ShouldReturnView()
        {
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(false);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = userController.Login() as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [TestCase("", "")]
        [TestCase(null, null)]
        public async Task POST_Login_UserEmailAndPasswordIsNullorWhitespace_ShouldReturnView(string email, string password)
        {
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(false);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var user = new User { Email = email, Password = password };

            var result = await userController.Login(user) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task POST_Login_UserDoesNotExist_ShouldReturnView()
        {
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(false);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var user = new User { Email = "zxcv@zxcv.com", Password = "asdfasdf9(" };

            var result = await userController.Login(user) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task POST_Login_UserPasswordIsNotEqualThePasswordFromDb_ShouldReturnView()
        {
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(false);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var user = new User { Email = "q@q.com", Password = "asdfasdf9(" };

            var result = await userController.Login(user) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task POST_Login_UserExists_ShouldRedirectToAction()
        {
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(false);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var user = new User { Email = "q@q.com", Password = "lalalala1!" };

            var result = await userController.Login(user) as RedirectToActionResult;

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("GetById", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
        }

        [Test]
        public void GET_Register_UserIsAuthenticated_ShouldReturnRedirectToAction()
        {
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(true);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = userController.Register() as RedirectToActionResult;

            Assert.AreEqual("GetById", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void GET_Register_UserIsNotAuthenticated_ShouldReturnView()
        {
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(false);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = userController.Register() as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [TestCase("", "")]
        [TestCase(null, null)]
        public async Task POST_Register_EmailOrPasswordIsNullOrWhitespace_ShouldReturnView(string email, string password)
        {
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(false);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var user = new User { Id = 5, Email = email, Password = password };

            var result = await userController.Register(user) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }


        //TODO POST REGISTER


        //TODO LOgout

        //TODO UPDATE GET

        //TODO UPDATE PUT

    }
}