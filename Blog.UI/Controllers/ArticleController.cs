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

        [HttpGet]
        public ActionResult<List<Dictionary<Article, List<Comment>>>> GetAll()
        {
            return _articleController.GetAll();
        }

        [HttpGet("{id}", Name = "GetArticle")]
        public ActionResult<Tuple<Article, List<Comment>>> GetById(int id)
        {
            return _articleController.GetById(id);
        }

<<<<<<< HEAD
        [HttpGet("Create")]
        public IActionResult CreateView()
        {
            return View("~/Views/Article/Create.cshtml");
        }

        [HttpPost("Create")]
        public IActionResult Create([FromForm] Article article)
        {
            article.UserId = int.Parse(User.Identity.Name);
            var response = _articleController.Create(article);
            if (response.GetType() != typeof(NoContentResult))
                return NotFound();
            return Ok();
            //return RedirectToAction("GetUser", "User", new { id = User.Identity.Name }); 
=======
        [HttpGet]
        public IActionResult CreateView()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Create([FromForm] Article article)
        {
            return _articleController.Create(article);
>>>>>>> 985bfe7c4ee18457b422a6e3096a96a463e2690f
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