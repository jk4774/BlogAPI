using Blog.API.Models;
using Blog.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
//using System.Net.Http.Headers;
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

            using (var client = new HttpClient())
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newUser.Token);

            //using (var req = new HttpRequestMessage())
            //{
            //    req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    req.Headers.Add("Authentication", string.Format("Bearer {0}", newUser.Token));
            //}

            //var bearerToken = new AuthenticationHeaderValue("Bearer " + newUser.Token);
            //using (var client = new HttpClient())
            //    client.DefaultRequestHeaders.Add("Authentication", string.Format("Bearer {0}", newUser.Token));

            return Content(newUser.Token);
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