using NUnit.Framework;
using BlogMvc.Controllers;
using BlogServices;
using BlogContext;
using FakeItEasy;

namespace BlogTests
{
    public class UserControllerTests
    { 
        private Blog _blog;
        private UserService _userService;
        private UserController _userController;
        [SetUp]
        public void Setup()
        {
            _blog = A.Fake<Blog>();
            _userService = A.Fake<UserService>();
            _userController = A.Fake<UserController>();
        }
    }
}