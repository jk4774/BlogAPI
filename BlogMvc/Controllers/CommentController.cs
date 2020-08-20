using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlogContext;
using BlogEntities;
using BlogServices;
using System.Threading.Tasks;
using System;
using System.Security.Claims;
using System.Linq;

namespace BlogMvc.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class CommentController : Controller
    {
        private int? _articleId;
        private readonly Blog _blog; 
        private readonly CommentService _commentService;  
        public CommentController(Blog blog, CommentService commentService)
        {
            _blog = blog;
            _commentService = commentService;
        }

        [HttpGet("Add/{id}")]
        public IActionResult Add(int id)
        {
            _articleId = id;
            return View();
        }

        [HttpPost("Add/{id}")]
        public async Task<IActionResult> Add(int id, [FromBody] Comment comment)
        {
            if (!ModelState.IsValid)
                return View();

            if (_articleId != id || !_blog.Articles.Any(x => x.Id == _articleId))
                return NotFound();

            comment.ArticleId = _articleId.Value;
            comment.UserId = int.Parse(User.Identity.Name);
            comment.Author = User.FindFirst(ClaimTypes.Email).Value;
            comment.Date = DateTime.Now;

            _blog.Comments.Add(comment);
            await _blog.SaveChangesAsync();

            return RedirectToAction("GetById", new { id = User.Identity.Name });
        }

        [HttpGet("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var comment = await _blog.Comments.FindAsync(id);
            if (comment == null || comment.UserId.ToString().Equals(User.Identity.Name))
                return NotFound();
            return View(comment);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] Comment updatedComment)
        {
            if (!ModelState.IsValid)
                return View(updatedComment);

            var comment = await _blog.Comments.FindAsync(id);
            if (comment == null || comment.UserId.ToString().Equals(User.Identity.Name))
                return NotFound();

            comment.Content = updatedComment.Content;
            comment.Date = DateTime.Now;

            _blog.Comments.Update(comment);
            await _blog.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _blog.Comments.FindAsync(id);
            if (comment == null || !comment.UserId.ToString().Equals(User.Identity.Name))
                return NotFound();

            _blog.Comments.Remove(comment);
            await _blog.SaveChangesAsync();

            return NoContent();
        }
    }
}
