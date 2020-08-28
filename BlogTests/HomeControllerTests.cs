using Microsoft.AspNetCore.Mvc;
using BlogMvc.Controllers;
using NUnit.Framework;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using FakeItEasy;

namespace BlogTests
{
    public class HomeControllerTests
    {
        private HomeController _homeController;

        [SetUp]
        public void Setup()
        {
            _homeController =  new HomeController();
        }

        [Test]
        public void GET_Index_ReturnsRedirectToAction()
        {
            var result = _homeController.Index() as RedirectToActionResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void GET_Index_ReturnsView()
        {
            // var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            // {
            //     new Claim(ClaimTypes.Name, "example name"),
            //     new Claim(ClaimTypes.NameIdentifier, "1"),
            //     new Claim("custom-claim", "example claim value"),
            // }, "mock"));
            
            // var homeController = new HomeController() { HttpContext = null } ;
            // homeController.HttpContext = new ControllerContext()
            // {
            //     HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            // }
            // var controller = new SomeController(dependencies…);
            // controller.ControllerContext = new ControllerContext()
            // {
            //     HttpContext = new DefaultHttpContext() { User = user }
            // };
            
            // var result = _homeController.Index() as ViewResult;

            // Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void GET_Error_WhetherResultView_ReturnsView()
        {       
            var result = _homeController.Error() as ViewResult;   
            
            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}