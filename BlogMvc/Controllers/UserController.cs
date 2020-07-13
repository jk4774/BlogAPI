using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using BlogMvc.Models;
using BlogServices;
using BlogEntities;
using BlogContext;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<User>> GetById(int id)
        {
            var articles = await _blog.Articles.ToListAsync();
            var user = await _blog.Users.FindAsync(id);
            if (user == null) 
                return RedirectToAction("Index", "Home");

            return await Task.Run(() => View ("~/Views/User/Main.cshtml", new UserViewModel { User = user, Articles = articles }));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] User user)
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

            var userIdentity = new ClaimsIdentity(userClaims, "CookieAuth");
            var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });

            await HttpContext.SignInAsync("CookieAuth", userPrincipal);

            HttpContext.User = userPrincipal;

            return RedirectToAction("GetUser", new { id = int.Parse(HttpContext.User.Identity.Name) });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] User user)
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
            var result = await _blog.SaveChangesAsync();
            
            if (result != 1) 
                return NotFound("Cannot add user to db");

            await _userService.Auth(user);
            
            return RedirectToAction("GetUser", new { id = int.Parse(HttpContext.User.Identity.Name) });
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return await Task.Run(() => RedirectToAction("Index", "Home"));
        }

        [HttpGet("Update")]
        public async Task<IActionResult> UpdateView()
        {
            return await Task.Run(() => View());
            // return View("~/Views/User/Update.cshtml", new User { Id = int.Parse(User.Identity.Name) });
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PasswordViewModel password)
        {
            return await Task.Run(() => View());
            // return _userController.UpdatePassword(id, password)
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
