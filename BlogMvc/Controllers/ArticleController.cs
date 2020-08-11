using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlogContext;
using BlogEntities;
using BlogServices;
using System.Security.Claims;

namespace BlogMvc.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ArticleController : Controller
    {
        private readonly Blog _blog;
        private readonly ArticleService _articleService;
        public ArticleController(Blog blog, ArticleService articleService)
        {
            _blog = blog;
            _articleService = articleService;
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
            article.AuthorEmail = User.FindFirst(ClaimTypes.Email).Value;

            await _blog.Articles.AddAsync(article);
            await _blog.SaveChangesAsync();

            return RedirectToAction("GetById", "User", new { id = User.Identity.Name });
        }

        [HttpGet("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var article = await _blog.Articles.FindAsync(id);
            if (article == null) 
                return NotFound();
            return View();
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] Article updatedArticle)
        {
            if (!ModelState.IsValid)
                return View();

            var article = await _blog.Articles.FindAsync(id);
            if (article == null)
                return NotFound();

            article.Title = updatedArticle.Title;
            article.Content = updatedArticle.Content;
            article.Date = updatedArticle.Date;

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
            
            _blog.Articles.Remove(article);
            var comments = _blog.Comments.Where(x => x.ArticleId == article.Id).ToList();
            if (comments.Count > 0)
                _blog.Comments.RemoveRange(comments);

            await _blog.SaveChangesAsync();

            return NoContent();
        }        
    }
}
