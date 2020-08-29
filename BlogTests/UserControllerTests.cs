using System;
using System.Threading.Tasks;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlogMvc.Models;
using BlogMvc.Controllers;
using BlogEntities;
using BlogServices;
using BlogContext;
using FakeItEasy;
using NUnit.Framework;

namespace BlogTests
{
    public class UserControllerTests
    {
        private Blog blog;
        private IHttpContextAccessor httpContextAccessor; 
        private DefaultHttpContext context;
        private GenericIdentity fakeIdentity;
        private GenericPrincipal principal;
        private UserService fakeUserService;

        private void SetupDb()
        {
            var guid = new Guid().ToString().Substring(0, 8).ToString();
            blog = new Blog(new DbContextOptionsBuilder<Blog>().UseInMemoryDatabase(guid).Options);
            blog.Users.Add(new User { Id = 1, Email = "q@q.com", Password = "lalalala1!" });
        }

        [SetUp]
        public void Setup()
        {
            SetupDb();

            httpContextAccessor = A.Fake<IHttpContextAccessor>();
            context = new DefaultHttpContext();
            fakeIdentity = A.Fake<GenericIdentity>();
            principal = A.Fake<GenericPrincipal>();
            fakeUserService = A.Fake<UserService>();

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);

            A.CallTo(() => fakeUserService.SignIn(A.Fake<User>())).DoesNothing();
            A.CallTo(() => fakeUserService.SingOut()).DoesNothing();
        }

        [Test]
        public async Task GET_GetById_IncorrectId_ShouldReturnRedirectToAction()
        {
            A.CallTo(() => fakeIdentity.Name).Returns("1");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);
            
            var userController = new UserController(blog, fakeUserService)
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
            A.CallTo(() => fakeIdentity.Name).Returns("2");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(blog, fakeUserService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = await userController.GetById(2) as RedirectToActionResult;

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

            var userController = new UserController(blog, fakeUserService)
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
            
            var userController = new UserController(blog, fakeUserService)
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

            var userController = new UserController(blog, fakeUserService)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = userController.Login() as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        //TODO POST Login

        [Test]
        public void GET_Register_UserIsAuthenticated_ShouldReturnRedirectToAction()
        {
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(true);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var userController = new UserController(blog, fakeUserService)
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

            var userController = new UserController(blog, fakeUserService)
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