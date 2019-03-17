using Blog.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using APIController = Blog.API.Controllers;

namespace Blog.UI.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class ArticleController : Controller
    {
        private readonly APIController.ArticleController _articleController;
        
        public ArticleController(BlogContext blogContext)
        {
            _articleController = new APIController.ArticleController(blogContext);
        }

        [HttpGet("{id}", Name = "GetArticle")]
        public ActionResult<Tuple<Article, List<Comment>>> GetById(int id)
        {
            return _articleController.GetById(id);
        }

        [HttpGet("Create")]
        public IActionResult CreateView()
        {
            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();
            return View("~/Views/Article/Create.cshtml");
        }

        [HttpPost("Create")]
        public IActionResult Create([FromForm] Article article)
        {
            article.Date = DateTime.UtcNow;
            var response = _articleController.Create(article);
            if (response.GetType() != typeof(NoContentResult))
            {
                TempData["Message"] = "Cannot add article, something went wrong.";
                return RedirectToAction("Create", "Article");
            }
            return RedirectToAction("Index", "Home");
        }
        
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromForm] Article updatedArticle)
        {
            return _articleController.Update(id, updatedArticle);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return _articleController.Delete(id);
        }
    }
}