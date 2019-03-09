using Microsoft.AspNetCore.Mvc;
using Blog.API.Models;
using System.Linq;
using DevOne.Security.Cryptography.BCrypt;
using Microsoft.AspNetCore.Authorization;
using Blog.API.Services;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly BlogContext _blogContext;
        private readonly UserService _userService;

        public UserController(BlogContext blogContext, UserService userService)
        {
            _blogContext = blogContext;
            _userService = userService;
        }

        public void DeleteUsers()
        {
            if (_blogContext.Users.Count() > 0)
            {
                foreach (var User in _blogContext.Users)
                    _blogContext.Users.Remove(User);
                _blogContext.SaveChanges();
            }
        }

        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult<User> GetById(int id)
        {
            var user = _blogContext.Users.Find(id);
            if (user == null)
                return NotFound();
            return new User { Id = user.Id, Name = user.Name, Password = "", Email = user.Email };
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult<User> Login([FromBody] User user)
        {
            if (user == null)
                return NotFound();

            if (new[] { user.Name, user.Password }.Any(x => string.IsNullOrWhiteSpace(x)))
                return NotFound();

            var userFromDatabase = _blogContext.Users.SingleOrDefault(x => x.Name.ToLower() == user.Name.ToLower());
            if (userFromDatabase == null)
                return NotFound();

            if (!BCryptHelper.CheckPassword(user.Password, userFromDatabase.Password))
                return NotFound();

            return _userService.Authenticate(userFromDatabase);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromBody] User user)
        {
            if (user == null)
                return NotFound();

            if (new[] { user.Name, user.Password, user.Email }.Any(x => string.IsNullOrWhiteSpace(x)))
                return NotFound();

            foreach (var User in _blogContext.Users)
                if (User.Name.ToLower() == user.Name.ToLower() || User.Email.ToLower() == user.Email.ToLower())
                    return NotFound();

            var encryptedPassword = BCryptHelper.HashPassword(user.Password, BCryptHelper.GenerateSalt(12));
            var newUser = new User { Id = user.Id, Name = user.Name, Password = encryptedPassword, Email = user.Email };

            _blogContext.Users.Add(newUser);
            _blogContext.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = _blogContext.Users.Find(id);
            if (new[] { user, updatedUser }.Any(x => x == null))
                return NotFound();

            if (new[] { updatedUser.Name, updatedUser.Password, updatedUser.Email }.Any(x => string.IsNullOrWhiteSpace(x)))
                return NotFound();

            if (_blogContext.Users.Any(x => x.Name.ToLower() == updatedUser.Name.ToLower()))
                return NotFound();

            if (_blogContext.Users.Any(x => x.Email.ToLower() == updatedUser.Email.ToLower()))
                return NotFound();

            var encryptedPassword = BCryptHelper.HashPassword(updatedUser.Password, BCryptHelper.GenerateSalt(12));

            user.Name = updatedUser.Name;
            user.Password = encryptedPassword;
            user.Email = updatedUser.Email;
            _blogContext.Users.Update(user);
            _blogContext.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePassword(int id, [FromBody] Password password)
        {
            var user = _blogContext.Users.Find(id);
            if (user == null || password == null)
                return NotFound();

            if (new [] { password.Old, password.New }.Any(x => string.IsNullOrWhiteSpace(x))) 
                return NotFound();

            if (!BCryptHelper.CheckPassword(password.Old, user.Password))
                return NotFound();

            var encryptedPassword = BCryptHelper.HashPassword(password.New, BCryptHelper.GenerateSalt(12));

            user.Password = encryptedPassword;
            _blogContext.Users.Update(user);
            _blogContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _blogContext.Users.Find(id);
            if (user == null)
                return NotFound();

            _blogContext.Users.Remove(user);
            _blogContext.SaveChanges();
            return NoContent();
        }
    }
}