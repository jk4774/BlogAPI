using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlogMvc.Models;
using Microsoft.AspNetCore.Authorization;
using BlogEntities;
using BlogContext;

namespace BlogMvc.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly Blog _blog;
        public UserController(Blog blog)
        {
            _blog = blog;
        }

        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult<User> GetById(int id)
        {
            return View();
            // if (_blog.Users.Count() == 0 && Request.Cookies["access_token"] != null)
            // {
            //     Response.Cookies.Delete("access_token");
            //     return RedirectToAction("Index", "Home");
            // }

            // var getArticles = _articleController.GetAll();
            // var response = _userController.GetById(id);
            // if (response.Value == null)
            // {
            //     TempData["Message"] = "Cannot find user with this id.";
            //     return RedirectToAction("Index", "Home");
            // }
            // return View("~/Views/User/Main.cshtml", new FullUser { User = response.Value, Articles = getArticles.Value });
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromForm] User user)
        {
            return View();
            // var response = _userController.Register(user);

            // if (response.GetType() != typeof(NoContentResult))
            // {
            //     TempData["Message"] = "Name length cannot be less than 4. <br /> " +
            //                           "Length of Password and Email cannot be less than 8. <br />" +
            //                           "User with this name or email already exist."; 
                                        
            //     return RedirectToAction("Index", "Home");
            // }

            // if (User.Identity.IsAuthenticated)
            //     return RedirectToAction("GetUser", new { id = User.Identity.Name });
            // TempData["Message"] = "$The user has been registered correctly.";
            // return RedirectToAction("Index", "Home");
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            return View();
            // Utils.DeleteCookie(HttpContext);
            // TempData["Message"] = "$User has been logged out.";
            // return RedirectToAction("Index", "Home");
        }

        [HttpGet("Update")]
        public IActionResult UpdateView()
        {
            return View();
            // return View("~/Views/User/Update.cshtml", new User { Id = int.Parse(User.Identity.Name) });
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] Password password)
        {
            return View();
            // return _userController.UpdatePassword(id, password);
        }

        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    if (id != int.Parse(User.Identity.Name))
        //        return NotFound();
        //    Utils.DeleteCookie(HttpContext);
        //    return _userController.Delete(id);
        //}

    }
}
