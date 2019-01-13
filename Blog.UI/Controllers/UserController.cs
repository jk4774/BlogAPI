using Blog.API.Models;
using Blog.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
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

        [EnableCors]
        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult<User> Login([FromForm] User user)
        {
            var userControllerLogin = _userController.Login(user);
            var newUser = userControllerLogin.Value;
            var status = userControllerLogin.Result;
            if (status != null && status.GetType() == typeof(NotFoundResult))
                return NotFound();

            var client = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new System.Uri("https://localhost:5001/user/login"),
                Headers = {
                    { HttpRequestHeaders.Authorization.ToString(), "Bearer " + newUser.Token },
                    { HttpRequestHeaders.Access.Tostring(), "application/json" },
                    { "X-Version", "1" }
                }
            };
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newUser.Token);

            return Ok();
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