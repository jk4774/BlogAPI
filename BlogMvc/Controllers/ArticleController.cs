using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlogContext;
using BlogEntities;

namespace BlogMvc.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ArticleController : Controller
    {
        private readonly BlogDbContext _blogDbContext;
        public ArticleController(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
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

            _blogDbContext.Articles.Add(article);
            await _blogDbContext.SaveChangesAsync();

            return RedirectToAction("GetById", "User", new { id = User.Identity.Name });
        }

        [HttpGet("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var article = await _blogDbContext.Articles.FindAsync(id);
            if (article == null || !article.UserId.ToString().Equals(User.Identity.Name))
                return NotFound();
            
            return View(article);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Article updatedArticle)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Article/Update.cshtml", updatedArticle);
                
            var article = await _blogDbContext.Articles.FindAsync(id);
            if (article == null || !article.UserId.ToString().Equals(User.Identity.Name))
                return NotFound();
                
            article.Title = updatedArticle.Title;
            article.Content = updatedArticle.Content;
            article.Date = DateTime.Now;

            _blogDbContext.Articles.Update(article);
            await _blogDbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _blogDbContext.Articles.FindAsync(id);
            if (article == null)
                return NotFound();
            
            if (!article.UserId.ToString().Equals(User.Identity.Name))
                return NotFound();
            
            _blogDbContext.Articles.Remove(article);
            var comments = _blogDbContext.Comments.Where(x => x.ArticleId == article.Id).ToList();
            if (comments.Count > 0)
                _blogDbContext.Comments.RemoveRange(comments);

            await _blogDbContext.SaveChangesAsync();

            return NoContent();
        }        
    }
}