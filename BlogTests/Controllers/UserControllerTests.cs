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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

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
            //A.CallTo(() => fakeBlog.Users.Any(x => x.))
            A.CallTo(() => fakeUserService.SignIn(A.Fake<User>())).DoesNothing();
            A.CallTo(() => fakeUserService.SingOut()).DoesNothing();
            A.CallTo(() => fakeUserService.Verify(
                  A<string>.That.Matches(x => x == testPassword),
                  A<string>.That.Matches(x => x == testPassword))).Returns(true);

            A.CallTo(() => fakeUserService.Verify(
                A<string>.That.Matches(x => x != testPassword),
                A<string>.That.Matches(x => x != testPassword))).Returns(false);

            A.CallTo(() => fakeUserService.Hash(A<string>.That.Matches(x => x == testPassword))).Returns(testPassword);
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

            var userController = new UserController(fakeBlog, fakeUserService, fakeArticleService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = await userController.Register(user) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        //register

//        if (_blogDbContext.Users.Any(i => i.Email.Equals(user.Email, StringComparison.CurrentCultureIgnoreCase)))
//            {
//                ModelState.AddModelError("error", "User with this email exists in db");
//                return View();
//    }

//    user.Password = _userService.Hash(user.Password);
            
//            _blogDbContext.Users.Add(user);
//            _blogDbContext.SaveChanges();
            
//            await _userService.SignIn(user);
            
//            return RedirectToAction("GetById", "User", new { id = User.Identity.Name
//});
     



        //TODO POST REGISTER


        //TODO LOgout

        //TODO UPDATE GET

        //TODO UPDATE PUT

    }
}