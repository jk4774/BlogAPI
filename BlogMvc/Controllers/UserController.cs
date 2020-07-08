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
            if (_blog.Users.Count() == 0 )
            {
                return RedirectToAction("Index", "Home");
            }

            var articles = _blog.Articles.ToList();
            var user = _blog.Users.Find(id);
            
            if (user == null) 
            {
                // there is no user with this id 
                return RedirectToAction("Index", "Home");
            }

            return View ("~/Views/User/Main.cshtml", new UserViewModel() { User = user, Articles = articles });

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
        [HttpPost("Login")]
        public IActionResult Login([FromForm] User user)
        {
            // if (HttpContext.User.Identity.IsAuthenticated)
            // {   

            // }

            if (!ModelState.IsValid)
            {
                // var validationErrors = 
                //     ModelState.Values.SelectMany(x => x.Errors).Select(o => o.ErrorMessage);
                return NotFound(ModelState);
            }

            if (!_userService.CheckPassword(user))
            {
                return NotFound("Wrong password");
            }

            HttpContext.SignInAsync();


            // validation user
            // check in db if exist == true
                //  SignIn() -> /user/:id
            // else
                //  Go to main page (wrong credentials)
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromForm] User user)
        {
            if (!ModelState.IsValid)
            {  
                var validationErrors = 
                    ModelState.Values.SelectMany(x => x.Errors).Select(o => o.ErrorMessage);

                // return NotFound("Model is not valid");
            }

            // if (users.Any(i => i.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase))) 
            // {
            //     return NotFound("User with this email is existing in db");
            // }

            var encryptedPassword = user.Password.ToString();
            user.Password = encryptedPassword;

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

            return Ok("Everything is good");

            // return RedirectToAction("GetUser", new { id = HttpContext.User.Name  });

            // return RedirectToAction("Index", "Home");

            // return View();
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
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
            
            // return View();
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
