using Blog.API.Models;
using Blog.API.Services;
using Blog.UI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using APIController = Blog.API.Controllers;

namespace Blog.UI.Controllers
{
    [Route("[controller]")]
    [Authorize]
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
            var response = _userController.GetById(id);
            if (response.Value == null)
                return RedirectToAction("Index", "Home");
            return View("~/Views/User/GetUser.cshtml", response.Value);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromForm] User user)
        {
            var response = _userController.Register(user);

            if (response.GetType() != typeof(NoContentResult))
                return RedirectToAction("Index", "Home");

            if (User.Identity.IsAuthenticated)
                return RedirectToAction("GetUser", new { id = User.Identity.Name });
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            Utils.DeleteCookie(Request, Response);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Update")]
        public IActionResult UpdateUserView()
        {
            return View("~/Views/User/UpdateUser.cshtml");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromForm] User updatedUser)
        {
            var response = _userController.Update(id, updatedUser);
            if (response.GetType() != typeof(NotFoundResult))
            {

            }
            return response;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (User.Identity.Name != id.ToString())
                return RedirectToAction("Index", "Home");

            var response = _userController.Delete(id);

            Utils.DeleteCookie(Request, Response);

            return RedirectToAction("Index", "Home");
        }
    }
}