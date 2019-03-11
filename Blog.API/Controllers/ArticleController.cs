using System;
using System.Collections.Generic;
using System.Linq;
using Blog.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArticleController : ControllerBase
    {
        private readonly BlogContext _blogContext;

        public ArticleController(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        public void DeleteArticles()
        {
            if (_blogContext.Articles.Count() > 0)
            {
                foreach (var Article in _blogContext.Articles)
                    _blogContext.Articles.Remove(Article);
                _blogContext.SaveChanges();
            }
        }

        [HttpGet]
        public ActionResult<List<Dictionary<Article, List<Comment>>>> GetAll()
        {
            if (_blogContext.Articles == null || _blogContext.Articles.Count() == 0)
                return NotFound();

            var List = new List<Dictionary<Article, List<Comment>>> { };
            foreach (var Article in _blogContext.Articles)
            {
                var Dictionary = new Dictionary<Article, List<Comment>> { { Article, new List<Comment> { } } };
                foreach (var Comment in _blogContext.Comments.Where(x => x.ArticleId == Article.Id))
                    Dictionary.Last().Value.Add(Comment);
                List.Add(Dictionary);
            }

            return List;
        }

        [HttpGet("{id}", Name = "GetArticle")]
        public ActionResult<Tuple<Article, List<Comment>>> GetById(int id)
        {
            var article = _blogContext.Articles.Find(id);
            if (article == null)
                return NotFound();

            var comments = _blogContext.Comments.Where(x => x.ArticleId == article.Id).ToList();
            if (comments.Count() == 0 || comments == null)
                comments = new List<Comment> { };

            return new Tuple<Article, List<Comment>>(article, comments);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Article article)
        {
            if (article == null)
                return NotFound();

            if (new[] { article.Title, article.Content }.Any(x => string.IsNullOrWhiteSpace(x)))
                return NotFound();

            _blogContext.Articles.Add(article);
            _blogContext.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Article updatedArticle)
        {
            var article = _blogContext.Articles.Find(id);
            if (article == null)
                return NotFound();

            if (new[] { updatedArticle.Title, updatedArticle.Content }.Any(x => string.IsNullOrWhiteSpace(x)))
                return NotFound();

            article.Title = updatedArticle.Title;
            article.Content = updatedArticle.Content;
            article.Date = DateTime.Now;
            _blogContext.Articles.Update(article);
            _blogContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var article = _blogContext.Articles.Find(id);
            var comments = _blogContext.Comments.Where(x => x.ArticleId == id);
            if (article == null)
                return NotFound();

            if (comments != null && comments.Count() > 0)
                foreach (var comment in comments)
                    _blogContext.Comments.Remove(comment);

            _blogContext.Articles.Remove(article);
            _blogContext.SaveChanges();
            return NoContent();
        }
    }
}