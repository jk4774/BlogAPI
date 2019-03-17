using System.Linq;
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
        private readonly APIController.ArticleController _articleController;
        private BlogContext _blogContext;
        public UserController(BlogContext blogContext, UserService userService)
        {
            _userController = new APIController.UserController(blogContext, userService);
            _articleController = new APIController.ArticleController(blogContext);
            _blogContext = blogContext;
        }

        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult<User> GetById(int id)
        {
            #region debug
            if (_blogContext.Users.Count() == 0 && Request.Cookies["access_token"] != null)
            {
                Response.Cookies.Delete("access_token");
                return RedirectToAction("Index", "Home");
            }
            #endregion
            var getArticles = _articleController.GetAll();
            var response = _userController.GetById(id);
            if (response.Value == null)
            {
                TempData["Message"] = "Cannot find user with this id.";
                return RedirectToAction("Index", "Home");
            }
            return View("~/Views/User/Main.cshtml", new FullUser { User = response.Value, Articles = getArticles.Value });
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromForm] User user)
        {
            var response = _userController.Register(user);
            if (response.GetType() != typeof(NoContentResult))
            {
                TempData["Message"] = "Cannot register, user with this name or email already exist.";
                return RedirectToAction("Index", "Home");
            }

            if (User.Identity.IsAuthenticated)
                return RedirectToAction("GetUser", new { id = User.Identity.Name });
            TempData["Message"] = "The user has been registered correctly.";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            Utils.DeleteCookie(HttpContext);
            TempData["Message"] = "User has been logged out.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Update")]
        public IActionResult UpdateView() 
        {
            return View("~/Views/User/Update.cshtml", new User { Id = int.Parse(User.Identity.Name) });
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] Password password)
        {
            var response = _userController.UpdatePassword(id, password);
            if (response.GetType() != typeof(NoContentResult))
            {
                TempData["Message"] = "Something went wrong, cannot update password.";
                return RedirectToAction("Update", "User"); 
            }

            TempData["Message"] = "Password has been changed correctly.";
            // , IsDanger = false };
            return RedirectToAction("Index", "Home");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (User.Identity.Name != id.ToString())
            {
                TempData["Message"] = "Something went wrong id has not passed.";
                return RedirectToAction("GetUser", new { id = User.Identity.Name });
            }
                
            var response = _userController.Delete(id);
            if (response.GetType() != typeof(NoContentResult))
            {
                TempData["Message"] = "Something went wrong, cannot delete user.";
                return RedirectToAction("GetUser", new { id = User.Identity.Name });
            }
                
            Utils.DeleteCookie(HttpContext);
            TempData["Message"] = "The user has been deleted correctly.";
            return RedirectToAction("Index", "Home");
        }
    }
}