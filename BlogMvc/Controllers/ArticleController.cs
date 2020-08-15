using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlogContext;
using BlogEntities;
using BlogServices;
using System.Security.Claims;
using System;

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
            article.Author = User.FindFirst(ClaimTypes.Email).Value;

            _blog.Articles.Add(article);
            await _blog.SaveChangesAsync();

            return RedirectToAction("GetById", "User", new { id = User.Identity.Name });
        }

        [HttpGet("Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var article = await _blog.Articles.FindAsync(id);
            if (article == null)
                return NotFound();
            
            return View(article);

            // var article = await _blog.Articles.FindAsync(id);
            // if (article == null)
            //     return NotFound();

            // if (!article.UserId.ToString().Equals(User.Identity.Name))
            //     return RedirectToAction("GetById", "User", new { id = User.Identity.Name });

            // return View(article);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] Article updatedArticle)
        {
            if (!ModelState.IsValid)
                return View(updatedArticle);
                
            var article = await _blog.Articles.FindAsync(id);
            if (article == null) 
            {
                ModelState.AddModelError("error", "An article with given id is null");
                return View(article); 
            }

            if (!article.UserId.ToString().Equals(User.Identity.Name))
            {
                ModelState.AddModelError("error", "You are not authorize to update the article");
                return View(article); 
            }
                
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