using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BlogMvc.Controllers;
using FakeItEasy;
using NUnit.Framework;

namespace BlogTests
{
    public class HomeControllerTests
    {
        [Test]
        public void Index_UserIsAuthenticated_ShouldReturnRedirectToAction()
        {
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeIdentity = A.Fake<GenericIdentity>();
            var principal = A.Fake<GenericPrincipal>();

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(true);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var homeController = new HomeController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextAccessor.HttpContext,
                }
            };

            var result = homeController.Index() as RedirectToActionResult;

            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void Index_UserIsNotAuthenticated_ShouldReturnView()
        {
            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeIdentity = A.Fake<GenericIdentity>();
            var principal = A.Fake<GenericPrincipal>();

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            A.CallTo(() => fakeIdentity.IsAuthenticated).Returns(false);
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var homeController = new HomeController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextAccessor.HttpContext,
                }
            };

            var result = homeController.Index() as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Error_ErrorViewShouldBeReturned_ShouldReturnView()
        {
            var homeController = new HomeController();


            var ll = homeController.Error();

            var result = homeController.Error() as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}