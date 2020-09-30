using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlogContext;
using BlogData.Entities;
using BlogServices;

namespace BlogMvc.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ArticleController : Controller
    {
        private readonly IBlogDbContext _blogDbContext;
        private readonly ArticleService _articleService;
        public ArticleController(IBlogDbContext blogDbContext, ArticleService articleService)
        {   
            _blogDbContext = blogDbContext;
            _articleService = articleService;
        }

        [HttpGet("Add")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("Add")]
        public IActionResult Add([FromForm] Article article)
        {
            if (!ModelState.IsValid)
                return View();

            article.UserId = int.Parse(User.Identity.Name);
            article.Author = User.FindFirst(ClaimTypes.Email).Value;

            _blogDbContext.Articles.Add(article);
            _blogDbContext.SaveChanges();

            return RedirectToAction("GetById", "User", new { id = User.Identity.Name });
        }

        [HttpGet("Update/{id}")]
        public IActionResult Update(int id)
        {
            var article = _blogDbContext.Articles.Find(id);
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
            _blogDbContext.SaveChanges();

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
            
            _articleService.RemoveArticle(_blogDbContext, article);

            return NoContent();
        }        
    }
}