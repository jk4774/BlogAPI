using Blog.API.Models;
using Blog.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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
            if (!HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return Ok(_userController.GetById(id).Value);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromForm] User user)
        {
            var response = _userController.Register(user);
            if (response.GetType() == typeof(NotFoundResult))
                return Redirect("/");
            if (HttpContext.User.Identity.IsAuthenticated)
                return Ok("Jest zalogowany");
            else
                return Redirect("/");
        }

        [AllowAnonymous]
        [HttpGet("Logout")]//will be post
        public IActionResult Logout()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok("zalogowany");
            }
            return Ok("LoggedIKKE");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromForm] User updatedUser)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                //
            }
            //
            return _userController.Update(id, updatedUser);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                //

            }
            //
            return _userController.Delete(id);
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