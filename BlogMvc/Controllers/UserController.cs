using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlogServices;
using BlogContext;
using BlogData.Entities;
using BlogData.ViewModels;

namespace BlogMvc.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IBlogDbContext _blogDbContext;
        private readonly UserService _userService;
        private readonly ArticleService _articleService;

        public UserController(IBlogDbContext blogDbContext, UserService userService, ArticleService articleService)
        {
            _blogDbContext = blogDbContext;
            _userService = userService;
            _articleService = articleService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!User.Identity.Name.Equals(id.ToString()))
                return RedirectToAction("GetById", "User", new { id = User.Identity.Name });

            var user = await _blogDbContext.Users.FindAsync(id);
            if (user == null)
                return RedirectToAction("Index", "Home");

            var articles = _articleService.GetArticleViewModels(_blogDbContext);

            return View("~/Views/User/Main.cshtml", new UserViewModel { User = user, ArticleViewModel = articles });
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

            var userDb = await _blogDbContext.Users.FindAsync(user.Email);
            if (userDb == null)
            {
                ModelState.AddModelError("error", "User does not exist");
                return View();
            }

            if (!_userService.Verify(user.Password, userDb.Password))
            {
                ModelState.AddModelError("error", "Wrong Password");
                return View();
            }

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

            if (_userService.IsEmailAvailable(_blogDbContext, user.Email))
            {
                ModelState.AddModelError("error", "User with this email exists in db");
                return View();
            }

            user.Password = _userService.Hash(user.Password);

            _blogDbContext.Users.Add(user);
            _blogDbContext.SaveChanges();

            await _userService.SignIn(user);

            return RedirectToAction("GetById", "User", new { id = User.Identity.Name });
        }

        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            await _userService.SingOut();
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
            if (!User.Identity.Name.Equals(id.ToString()))
                return NotFound();

            if (!ModelState.IsValid)
                return View();

            var userDb = await _blogDbContext.Users.FindAsync(id);
            if (userDb == null)
            {
                ModelState.AddModelError("error", "Unexpected error, user with this id does not exist");
                return View();
            }

            if (!_userService.Verify(password.Old, userDb.Password))
            {
                ModelState.AddModelError("error", "Old password is not equal");
                return View();
            }

            userDb.Password = _userService.Hash(password.New);

            _blogDbContext.Users.Update(userDb);
            _blogDbContext.SaveChanges();

            return NoContent();
        }
    }
}