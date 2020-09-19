using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlogData.Entities;
using BlogData.ViewModels;
using BlogContext;
using BlogServices;
using BlogMvc.Controllers;
using BlogFakes;
using FakeItEasy;
using NUnit.Framework;

namespace BlogTests.Controllers
{
    public class UserControllerTests
    {
        private readonly string testPassword = "lalalala1!";
        private readonly User user = new User { Id = 1, Email = "q@q.com", Password = "lalalala1!" };
        private readonly Article article = new Article { Id = 1, UserId = 1, Author = "q@q.com", Content = "test-article-content", Date = DateTime.Now, Title = "test-article-title" };
        private readonly Comment comment = new Comment { Id = 1, ArticleId = 1, UserId = 1, Date = DateTime.Now, Content = "test-comment-content", Author = "q@q.com" };
        
        private List<User> Users => new List<User> { user };
        private List<Article> Articles => new List<Article> { article };
        private List<Comment> Comments => new List<Comment> { comment };

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
            var fakeUserDbSet = new FakeUserDbSet() { data = new ObservableCollection<User>(Users) };
            var fakeArticleDbSet = new FakeArticleDbSet() { data = new ObservableCollection<Article>(Articles) };
            var fakeCommentDbSet = new FakeCommentDbSet() { data = new ObservableCollection<Comment>(Comments) };

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
                    Comments = Comments 
                }
            });

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            
            A.CallTo(() => fakeUserService.SignIn(A.Fake<User>())).DoesNothing();
            A.CallTo(() => fakeUserService.SingOut()).DoesNothing();
            A.CallTo(() => fakeUserService.Verify(
                  A<string>.That.Matches(x => x == testPassword),
                  A<string>.That.Matches(x => x == testPassword))).Returns(true);

            A.CallTo(() => fakeUserService.Verify(
                A<string>.That.Matches(x => x != testPassword),
                A<string>.That.Matches(x => x != testPassword))).Returns(false);
            A.CallTo(() => fakeUserService.Hash(A<string>.That.Matches(x => x == testPassword))).Returns(testPassword);

            A.CallTo(() => fakeUserService.GetUserByEmail(fakeBlog, A<string>.That.Matches(x => x == user.Email))).Returns(user);
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

        [Test]
        public async Task POST_Register_UserWithThisMailExists_ShouldReturnView()
        {
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(false);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);
            A.CallTo(() => fakeUserService.IsEmailAvailable(fakeBlog, A<string>.That.Matches(x => x.Equals("q@q.com")))).Returns(true);

            var userController = new UserController(fakeBlog, fakeUserService, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = await userController.Register(user) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task POST_Register_CreateANewUser_ShouldReturnRedirectToAction()
        {
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(false);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);
            A.CallTo(() => fakeUserService.IsEmailAvailable(fakeBlog, A<string>.That.Matches(x => !x.Equals("q@q.com")))).Returns(false);

            var userController = new UserController(fakeBlog, fakeUserService, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = await userController.Register(new User { Id = 4, Email = "w@w.com", Password = "lalalala1!" }) as RedirectToActionResult;

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("GetById", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
        }

        [Test]
        public async Task POST_LogOut_UserLogOut_ShouldReturnRedirectToAction()
        {
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(false);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = await userController.LogOut() as RedirectToActionResult;

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
        }

        [Test]
        public void GET_Update_UpdateViewShouldBeReturned_ShouldReturnView()
        {
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(false);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = userController.Update() as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task PUT_Update_IncorrectId_ShouldReturnNotFound()
        {
            A.CallTo(() => fakeIdentity.Name).Returns("1");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = await userController.Update(2, new PasswordViewModel { Old = "lalalala1!", New = "lalalala2!" }) as NotFoundResult;

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task PUT_Update_UserWithThisIdDoesNotExistInDb_ShouldReturnView()
        {
            A.CallTo(() => fakeIdentity.Name).Returns("4");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = await userController.Update(4, new PasswordViewModel { Old = "lalalala1!", New = "lalalala2!" }) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task PUT_Update_OldPasswordIsNotMatchingTheOldPasswordFromDb_ShouldReturnView()
        {
            A.CallTo(() => fakeIdentity.Name).Returns("1");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = await userController.Update(1, new PasswordViewModel { Old = "lalalala2!", New = "lalalala3!" }) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task PUT_Update_UserExistsAndPasswordViewModelIsCorrect_ShouldReturnNoContent()
        {
            A.CallTo(() => fakeIdentity.Name).Returns("1");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = await userController.Update(1, new PasswordViewModel { Old = "lalalala1!", New = "lalalala3!" }) as NoContentResult;

            Assert.IsInstanceOf<NoContentResult>(result);
        }
    }
}