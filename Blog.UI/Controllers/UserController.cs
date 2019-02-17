using Blog.API.Models;
using Blog.API.Services;
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
            if (!User.Identity.IsAuthenticated)
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
            if (Request.Cookies["access_token"] != null)
            {
                try { Response.Cookies.Delete("access_token"); }
                catch { throw new Exception("Cannot delete cookie, something went wrong"); }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromForm] User updatedUser)
        {
            var response = _userController.Update(id, updatedUser);
            return response;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = _userController.Delete(id);
            if (response.GetType() != typeof(NoContentResult))
                return Ok("Something went wrong"); 
            return response;
        }
    }
}

#region old
        //[AllowAnonymous]
        //[HttpPost("Login")]
        //public ActionResult<User> Login([FromBody] User user)
        //{
        //    var response = _userController.Login(User);
        //    if (response.GetType() == typeof(NotFoundResult))
        //        return 
        //    if (IsLogged)
        //    {
        //        var idd = HttpContext.User.Identity.Name;
        //        return Ok(idd);
        //        //return _userController.Login(user);
        //    }
        //    return Ok();
        //    //return _userController.Login(user);
        //}
        #endregion