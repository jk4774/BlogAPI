using Blog.API.Models;
using Blog.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using APIController = Blog.API.Controllers;

namespace Blog.UI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : Controller
    {
        private readonly APIController.UserController _userController;

        public UserController(BlogContext blogContext, UserService userService)
        {
            _userController = new APIController.UserController(blogContext, userService);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult<User> GetById(int id)
        {
            return _userController.GetById(id);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult<User> Login([FromForm] User user)
        {
            return _userController.Login(user);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromForm] User user)
        {
            return _userController.Register(user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromForm] User updatedUser)
        {
            return _userController.Update(id, updatedUser);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return _userController.Delete(id);
        }
    }
}