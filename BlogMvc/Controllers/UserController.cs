using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using BlogServices;
using BlogEntities;
using BlogContext;
using BlogMvc.Models;

namespace BlogMvc.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly Blog _blog;
        private readonly UserService _userService;

        public UserController(Blog blog, UserService userService)
        {
            _blog = blog;
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {   
            if (!User.Identity.Name.Equals(id.ToString())) 
                return RedirectToAction("GetById", "User", new { id = User.Identity.Name });

            var articles = await _blog.Articles.ToListAsync();
            var user = await _blog.Users.FindAsync(id);
            if (user == null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Home");
            }

            return View("~/Views/User/Main.cshtml", new UserViewModel { User = user, Articles = articles });
        }


        [AllowAnonymous]
        [HttpGet("Login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) 
                return RedirectToAction("GetById", "User", new { id = User.Identity.Name });
            return View();
        }

        [AllowAnonymous]
        [HttpPost("Login")] 
        public async Task<IActionResult> Login([FromForm] User user)
        {
            if (!ModelState.IsValid)
                return View();
                
            var userDb = await _blog.Users.FirstOrDefaultAsync(i => 
                i.Email.Equals(user.Email, StringComparison.CurrentCultureIgnoreCase));
            
            if (userDb == null)
                return NotFound("User does not exist");

            if (!_userService.Verify(user.Password, userDb.Password)) 
                return NotFound("Wrong password");

            await _userService.SignIn(userDb);

            return RedirectToAction("GetById", "User", new { id = User.Identity.Name });
        }

        [AllowAnonymous]
        [HttpGet("Register")]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) 
                return RedirectToAction("GetById", "User", new { id = User.Identity.Name });
            return View();
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] User user)
        {
            if (!ModelState.IsValid)
                return View();

            if (_blog.Users.Any(i => i.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
                return NotFound("User with this email is existing in db");
            
            user.Password = _userService.Hash(user.Password);
            
            _blog.Users.Add(user);
            _blog.SaveChanges();
            
            await _userService.SignIn(user);
            
            return RedirectToAction("GetById", "User", new { id = User.Identity.Name });
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); 
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Update")]
        public IActionResult Update()
        {
            return View();
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PasswordViewModel password)
        {
            if (!ModelState.IsValid)
                return View();

            var userDb = await _blog.Users.FirstOrDefaultAsync(x => x.Id.ToString() == User.Identity.Name);
            if (userDb == null) 
                return NotFound("Unexpected error, user with this id does not exist");

            var hashedOldPassword = _userService.Hash(password.Old);
            if (userDb.Equals(hashedOldPassword))
                return NotFound("");
            
            userDb.Password = _userService.Hash(password.New);

            _blog.Users.Update(userDb);
            _blog.SaveChanges();

            return RedirectToAction("GetById", "User", new { id = User.Identity.Name });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }
    }
}