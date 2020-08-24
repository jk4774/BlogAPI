using Microsoft.AspNetCore.Mvc;
using BlogMvc.Controllers;
using NUnit.Framework;

namespace BlogTests
{
    public class HomeControllerTests
    {
        private HomeController _homeController;

        [SetUp]
        public void Setup()
        {
            _homeController = new HomeController();
        }

        [Test]
        public void GET_Error_ShouldPass_View()
        {
            var error = _homeController.Error();

            Assert.IsInstanceOf<ViewResult>(error as ViewResult);
        }
    }
}