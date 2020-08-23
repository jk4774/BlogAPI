using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlogContext;
using BlogEntities;
using BlogServices;

namespace BlogMvc.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ArticleController : Controller
    {
        private readonly Blog _blog;
        public ArticleController(Blog blog)
        {
            _blog = blog;
        }

        [HttpGet("Add")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] Article article)
        {
            if (!ModelState.IsValid)
                return View();

            article.UserId = int.Parse(User.Identity.Name);
            article.Author = User.FindFirst(ClaimTypes.Email).Value;

            _blog.Articles.Add(article);
            await _blog.SaveChangesAsync();

            return RedirectToAction("GetById", "User", new { id = User.Identity.Name });
        }

        [HttpGet("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var article = await _blog.Articles.FindAsync(id);
            if (article == null || !article.UserId.ToString().Equals(User.Identity.Name))
                return NotFound();
            
            return View(article);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Article updatedArticle)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Article/Update.cshtml", updatedArticle);
                
            var article = await _blog.Articles.FindAsync(id);
            if (article == null || !article.UserId.ToString().Equals(User.Identity.Name))
                return NotFound();
                
            article.Title = updatedArticle.Title;
            article.Content = updatedArticle.Content;
            article.Date = DateTime.Now;

            _blog.Articles.Update(article);
            await _blog.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _blog.Articles.FindAsync(id);
            if (article == null)
                return NotFound();
            
            if (!article.UserId.ToString().Equals(User.Identity.Name))
                return NotFound();
            
            _blog.Articles.Remove(article);
            var comments = _blog.Comments.Where(x => x.ArticleId == article.Id).ToList();
            if (comments.Count > 0)
                _blog.Comments.RemoveRange(comments);

            await _blog.SaveChangesAsync();

            return NoContent();
        }        
    }
}