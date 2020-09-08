using System.Threading.Tasks;
using System.Security.Principal;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlogMvc.Models;
using BlogMvc.Controllers;
using BlogEntities;
using BlogServices;
using BlogContext;
using FakeItEasy;
using NUnit.Framework;
using System;
using BlogFakes;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace BlogTests
{
    public class UserControllerTests
    {
        private IBlogDbContext fakeBlog;

        [SetUp]
        public void Setup()
        {
            var fakeUsers = new List<User> { new User { Id = 1, Email = "q@q.com", Password = "lalalala1!" } };
            var fakeArticles = new List<Article> { new Article { Id = 1, UserId = 1, Author = "q@q.com", Content = "test-article-content", Date = DateTime.Now, Title = "test-article-title" } };
            var fakeComments = new List<Comment> { new Comment { Id = 1, ArticleId = 1, UserId = 1, Date = DateTime.Now, Content = "test-comment-content", Author = "q@q.com" } };

            var fakeUserDbSet = new FakeUserDbSet() { data = new ObservableCollection<User>(fakeUsers) };
            var fakeArticleDbSet = new FakeArticleDbSet() { data = new ObservableCollection<Article>(fakeArticles) };
            var fakeCommentDbSet = new FakeCommentDbSet() { data = new ObservableCollection<Comment>(fakeComments) };

            fakeBlog = A.Fake<IBlogDbContext>();

            var context = new FakeBlogDbContext()
            { 
                Users = fakeUserDbSet,
                Articles = fakeArticleDbSet,
                Comments = fakeCommentDbSet
            };

            A.CallTo(() => fakeBlog).Returns(context);
        }

        [Test]
        public async Task GET_GetById_IncorrectId_ShouldReturnRedirectToAction()
        {
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeIdentity = A.Fake<GenericIdentity>();
            var principal = A.Fake<GenericPrincipal>();
            var fakeUserService = A.Fake<UserService>();

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            A.CallTo(() => fakeUserService.SignIn(A.Fake<User>())).DoesNothing();
            A.CallTo(() => fakeUserService.SingOut()).DoesNothing();
            A.CallTo(() => fakeIdentity.Name).Returns("1");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService)
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
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeIdentity = A.Fake<GenericIdentity>();
            var principal = A.Fake<GenericPrincipal>();
            var fakeUserService = A.Fake<UserService>();

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            A.CallTo(() => fakeUserService.SignIn(A.Fake<User>())).DoesNothing();
            A.CallTo(() => fakeUserService.SingOut()).DoesNothing();
            A.CallTo(() => fakeIdentity.Name).Returns("1");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService)
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
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeIdentity = A.Fake<GenericIdentity>();
            var principal = A.Fake<GenericPrincipal>();
            var fakeUserService = A.Fake<UserService>();

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            A.CallTo(() => fakeUserService.SignIn(A.Fake<User>())).DoesNothing();
            A.CallTo(() => fakeUserService.SingOut()).DoesNothing();
            A.CallTo(() => fakeIdentity.Name).Returns("1");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService)
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
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeIdentity = A.Fake<GenericIdentity>();
            var principal = A.Fake<GenericPrincipal>();
            var fakeUserService = A.Fake<UserService>();

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            A.CallTo(() => fakeUserService.SignIn(A.Fake<User>())).DoesNothing();
            A.CallTo(() => fakeUserService.SingOut()).DoesNothing();
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(true);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService)
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
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeIdentity = A.Fake<GenericIdentity>();
            var principal = A.Fake<GenericPrincipal>();
            var fakeUserService = A.Fake<UserService>();

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            A.CallTo(() => fakeUserService.SignIn(A.Fake<User>())).DoesNothing();
            A.CallTo(() => fakeUserService.SingOut()).DoesNothing();
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(false);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService)
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
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeIdentity = A.Fake<GenericIdentity>();
            var principal = A.Fake<GenericPrincipal>();
            var fakeUserService = A.Fake<UserService>();

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            A.CallTo(() => fakeUserService.SignIn(A.Fake<User>())).DoesNothing();
            A.CallTo(() => fakeUserService.SingOut()).DoesNothing();
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(false);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService)
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
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeIdentity = A.Fake<GenericIdentity>();
            var principal = A.Fake<GenericPrincipal>();
            var fakeUserService = A.Fake<UserService>();

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            A.CallTo(() => fakeUserService.SignIn(A.Fake<User>())).DoesNothing();
            A.CallTo(() => fakeUserService.SingOut()).DoesNothing();
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(false);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService)
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
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeIdentity = A.Fake<GenericIdentity>();
            var principal = A.Fake<GenericPrincipal>();
            var fakeUserService = A.Fake<UserService>();

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            A.CallTo(() => fakeUserService.SignIn(A.Fake<User>())).DoesNothing();
            A.CallTo(() => fakeUserService.SingOut()).DoesNothing();
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(false);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            //blog.Users.Add(new User { Id = 1, Email = "q@q.com", Password = "lalalala1!" });

            var user = new User { Email = "q@q.com", Password = "asdfasdf9(" };

            var result = await userController.Login(user) as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task POST_Login_UserExists_ShouldRedirectToAction()
        {
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeIdentity = A.Fake<GenericIdentity>();
            var principal = A.Fake<GenericPrincipal>();
            var fakeUserService = A.Fake<UserService>();

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            A.CallTo(() => fakeUserService.SignIn(A.Fake<User>())).DoesNothing();
            A.CallTo(() => fakeUserService.SingOut()).DoesNothing();
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(false);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var user = new User { Email = "q@q.com", Password = "lalalala1!" };

            var result = await userController.Login(user) as RedirectToActionResult;

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("GetById", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
        }

        //         if (!ModelState.IsValid)
        //                return View();

        //        var userDb = await _blogDbContext.Users.FirstOrDefaultAsync(i =>
        //            i.Email.Equals(user.Email, StringComparison.CurrentCultureIgnoreCase));

        //            if (userDb == null)
        //            {
        //                ModelState.AddModelError("error", "User does not exist");
        //                return View();
        //    }

        //            if (!_userService.Verify(user.Password, userDb.Password)) 
        //            {
        //                ModelState.AddModelError("error", "Wrong Password");
        //                return View();
        //}

        //await _userService.SignIn(userDb);

        //return RedirectToAction("GetById", "User", new { id = User.Identity.Name });


        [Test]
        public void GET_Register_UserIsAuthenticated_ShouldReturnRedirectToAction()
        {
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeIdentity = A.Fake<GenericIdentity>();
            var principal = A.Fake<GenericPrincipal>();
            var fakeUserService = A.Fake<UserService>();

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            A.CallTo(() => fakeUserService.SignIn(A.Fake<User>())).DoesNothing();
            A.CallTo(() => fakeUserService.SingOut()).DoesNothing();
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(true);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService)
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
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeIdentity = A.Fake<GenericIdentity>();
            var principal = A.Fake<GenericPrincipal>();
            var fakeUserService = A.Fake<UserService>();

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            A.CallTo(() => fakeUserService.SignIn(A.Fake<User>())).DoesNothing();
            A.CallTo(() => fakeUserService.SingOut()).DoesNothing();
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(false);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(fakeBlog, fakeUserService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = userController.Register() as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }


        //TODO POST REGISTER


        //TODO LOgout

        //TODO UPDATE GET

        //TODO UPDATE PUT

    }
}