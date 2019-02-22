using Blog.API.Models;
using Blog.API.Services;
using Blog.UI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
            return View("~/Views/User/Main.cshtml", response.Value);
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
        public IActionResult UpdateView()
        {
            return View("~/Views/User/Update.cshtml");
        }

        [HttpPut("{id}", Name = "Update")]
        public IActionResult Update(int id, [FromForm] User updatedUser)
        {
            var user = _userController.GetById(id).Value;
            user.Password = updatedUser.Password;

            var response = _userController.Update(id, user);
            if (response.GetType() != typeof(NotFoundResult))
                return Redirect("");
            return response;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new Exception(User.Identity.Name + "  " + id);
            //if (User.Identity.Name != id.ToString())
            //    throw new Exception("Id is wrong");

            //var response = _userController.Delete(id);
            //if (response.GetType() != typeof(NoContentResult))
            //    return RedirectToAction("GetUser", new { id = User.Identity.Name });

            //Utils.DeleteCookie(Request, Response);
            //return RedirectToAction("Index", "Home");
        }
    }
}