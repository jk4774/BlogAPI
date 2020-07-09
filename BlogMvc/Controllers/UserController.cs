using System;
using Microsoft.AspNetCore.Mvc;
using BlogMvc.Models;
using Microsoft.AspNetCore.Authorization;
using BlogEntities;
using BlogContext;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using BlogServices;

namespace BlogMvc.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly Blog _blog;
        private readonly UserService _userService;
        public UserController(Blog blog, UserService userService)
        {
            _blog = blog;
            _userService = userService;
        }

        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult<User> GetById(int id)
        {
            var articles = _blog.Articles.ToList();
            var user = _blog.Users.Find(id);
            if (user == null) 
                return RedirectToAction("Index", "Home");

            return View ("~/Views/User/Main.cshtml", new UserViewModel { User = user, Articles = articles });
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromForm] User user)
        {
            if (!ModelState.IsValid)
                return NotFound(ModelState);

            var userDb = _blog.Users.FirstOrDefault(i => i.Email.Equals(user.Email, StringComparison.CurrentCultureIgnoreCase));
            if (userDb == null)
                return NotFound("User does not exist");

            var hashedPassword = _userService.HashPassword(user.Password);

            if (userDb.Password != hashedPassword) 
                return NotFound("Wrong password");

            var userClaims = new List<Claim>();
            userClaims.Add(new Claim(ClaimTypes.Name, userDb.Id.ToString()));

            var userIdentity = new ClaimsIdentity(userClaims);
            var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });

            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("GetUser", new { id = HttpContext.User.Identity.Name });
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromForm] User user)
        {
            if (!ModelState.IsValid)
            {  
                var validationErrors = 
                    ModelState.Values.SelectMany(x => x.Errors).Select(o => o.ErrorMessage);
                return NotFound(ModelState);
            }

            if (_blog.Users.Any(i => i.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase))) 
                return NotFound("User with this email is existing in db");

            user.Password = _userService.HashPassword(user.Password);

            _blog.Users.Add(user);
            var in2 = _blog.SaveChanges();
            
            if (in2 != 1) {
                return NotFound("Cannot add user to db");
            }

            var userClaims = new List<Claim>();
            userClaims.Add(new Claim(ClaimTypes.Name, user.Id.ToString()));

            var userIdentity = new ClaimsIdentity(userClaims);
            var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });

            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("GetUser", new { id = HttpContext.User.Identity.Name });
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Update")]
        public IActionResult UpdateView()
        {
            return View();
            // return View("~/Views/User/Update.cshtml", new User { Id = int.Parse(User.Identity.Name) });
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] PasswordViewModel password)
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
