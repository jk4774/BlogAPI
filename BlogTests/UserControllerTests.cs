using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogMvc.Controllers;
using BlogEntities;
using BlogServices;
using BlogContext;
using FakeItEasy;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using System;
using BlogMvc.Models;

namespace BlogTests
{
    public class UserControllerTests
    {
        private Blog blog;
        
        [SetUp]
        public void Setup()
        {
            var guid = new Guid().ToString().Substring(0, 8).ToString();
            var options = new DbContextOptionsBuilder<Blog>().UseInMemoryDatabase(guid);
            blog = new Blog(options.Options);
            blog.Users.Add(new User { Id = 1, Email = "q@q.com", Password = "lalalala1!" });
        }

        [Test]
        public async Task GetById_IncorrectId_ShouldReturnRedirectToAction()
        {
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeIdentity = A.Fake<GenericIdentity>();
            var principal = A.Fake<GenericPrincipal>();

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            A.CallTo(() => fakeIdentity.Name).Returns("1");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);
            
            var userService = new UserService(httpContextAccessor);
            var userController = new UserController(blog, userService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = await userController.GetById(10) as RedirectToActionResult;

            Assert.AreEqual("GetById", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public async Task GetById_CorrectIdUserDoesNotExistInDb_ShouldReturnRedirectToAction()
        {
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeIdentity = A.Fake<GenericIdentity>();
            var principal = A.Fake<GenericPrincipal>();

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            A.CallTo(() => fakeIdentity.Name).Returns("2");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userService = new UserService(httpContextAccessor);
            var userController = new UserController(blog, userService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = await userController.GetById(2) as RedirectToActionResult;

            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }


        [Test]
        public async Task GetById_CorrectIdUserDoesExist_ShouldReturnView()
        {
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeIdentity = A.Fake<GenericIdentity>();
            var principal = A.Fake<GenericPrincipal>();

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            A.CallTo(() => fakeIdentity.Name).Returns("1");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userService = new UserService(httpContextAccessor);
            var userController = new UserController(blog, userService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = await userController.GetById(1) as ViewResult;

            Assert.IsInstanceOf<UserViewModel>(result.ViewData.Model);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Login_UserIsAuthenticated_ShouldReturnRedirectToAction()
        {
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeIdentity = A.Fake<GenericIdentity>();
            var principal = A.Fake<GenericPrincipal>();

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(true);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);
            
            var userService = new UserService(httpContextAccessor);
            var userController = new UserController(blog, userService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = userController.Login() as RedirectToActionResult;

            Assert.AreEqual("GetById", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void Login_UserIsNotAuthenticated_ShouldReturnView()
        {
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeIdentity = A.Fake<GenericIdentity>();
            var principal = A.Fake<GenericPrincipal>();

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(false);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userService = new UserService(httpContextAccessor);
            var userController = new UserController(blog, userService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = userController.Login() as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        //TODO POST Login

        [Test]
        public void Register_UserIsAuthenticated_ShouldReturnRedirectToAction()
        {
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeIdentity = A.Fake<GenericIdentity>();
            var principal = A.Fake<GenericPrincipal>();

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(true);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userService = new UserService(httpContextAccessor);
            var userController = new UserController(blog, userService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = userController.Register() as RedirectToActionResult;

            Assert.AreEqual("GetById", result.ActionName);
            Assert.AreEqual("User", result.ControllerName);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void Register_UserIsNotAuthenticated_ShouldReturnView()
        {
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeIdentity = A.Fake<GenericIdentity>();
            var principal = A.Fake<GenericPrincipal>();

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(false);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userService = new UserService(httpContextAccessor);
            var userController = new UserController(blog, userService)
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