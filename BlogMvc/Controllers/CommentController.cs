using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlogContext;
using BlogEntities;
using BlogServices;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;

namespace BlogMvc.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class CommentController : Controller
    {
        private readonly Blog _blog; 
        public CommentController(Blog blog)
        {
            _blog = blog;
        }

        [HttpGet("Add/{id}")]
        public IActionResult Add(int id)
        {
            var comment = new Comment { ArticleId = id };
            if (!_blog.Articles.Any(x => x.Id == comment.ArticleId))
                return RedirectToAction("GetById", "User", new { id = User.Identity.Name });
            return View(comment);
        }

        [HttpPost("Add/{id}")]
        public async Task<IActionResult> Add(int id, [FromForm] Comment comment)
        {
            comment.ArticleId = id;

            if (!ModelState.IsValid)
                return View("~/Views/Comment/Add.cshtml", comment);

            if (!_blog.Articles.Any(x => x.Id == id))
                return NotFound();

            comment.UserId = int.Parse(User.Identity.Name);
            comment.Author = User.FindFirst(ClaimTypes.Email).Value;
         
            _blog.Comments.Add(comment);
            await _blog.SaveChangesAsync();

            return RedirectToAction("GetById", "User", new { id = User.Identity.Name });
        }

        [HttpGet("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var comment = await _blog.Comments.FindAsync(id);
            if (comment == null || !comment.UserId.ToString().Equals(User.Identity.Name))
                return RedirectToAction("GetById", "User", new { id = User.Identity.Name });
            return View(comment);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Comment updatedComment)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Comment/Update.cshtml", updatedComment);

            var comment = await _blog.Comments.FindAsync(id);
            if (comment == null || !comment.UserId.ToString().Equals(User.Identity.Name))
                return RedirectToAction("GetById", "User", new { id = User.Identity.Name });

            comment.Content = updatedComment.Content;

            _blog.Comments.Update(comment);
            await _blog.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _blog.Comments.FindAsync(id);
            if (comment == null || !comment.UserId.ToString().Equals(User.Identity.Name))
                return RedirectToAction("GetById", "User", new { id = User.Identity.Name });

            _blog.Comments.Remove(comment);
            await _blog.SaveChangesAsync();

            return NoContent();
        }
    }
}
