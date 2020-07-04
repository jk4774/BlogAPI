using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlogMvc.Models;
using Microsoft.AspNetCore.Authorization;
using BlogContext;
using BlogEntities;

namespace BlogMvc.Controllers
{
    [Authorize]
    public class ArticleController : Controller
    {
        private readonly Blog _blog;
        public ArticleController(Blog blog)
        {
            _blog = blog;
        }

        [HttpGet("{id}", Name = "GetArticle")]
        public ActionResult<Tuple<Article, List<Comment>>> GetById(int id)
        {
            return View();
            // return _articleController.GetById(id);
        }

        [HttpGet("Create")]
        public IActionResult CreateView()
        {
            return View();
            // if (TempData["Message"] != null)
            //     ViewBag.Message = TempData["Message"].ToString();
            // return View("~/Views/Article/Create.cshtml");
        }

        [HttpPost("Create")]
        public IActionResult Create([FromForm] Article article)
        {
            return View();
            // article.UserId = int.Parse(User.Identity.Name);
            // article.UserName = _blogContext.Users.Find(article.UserId).Name;
            // var response = _articleController.Create(article);
            // if (response.GetType() != typeof(NoContentResult))
            // {
            //     TempData["Message"] = "Cannot add article, something went wrong.";
            //     return RedirectToAction("Create", "Article");
            // }
            // return RedirectToAction("Index", "Home");
        }

        [HttpGet("Update/{id}")]
        public IActionResult UpdateView(int id)
        {
            return View();
            // var article = _blogContext.Articles.Find(id);
            // if (article == null)
            //     return RedirectToAction("Index", "Home");
            // if (article.UserId != int.Parse(User.Identity.Name))
            //     return RedirectToAction("Index", "Home");
            // if (TempData["Message"] != null)
            //     ViewBag.Message = TempData["Message"].ToString();
            // return View("~/Views/Article/Update.cshtml", new Article { Id = article.Id, Title = article.Title, Content = article.Content });
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] Article updatedArticle)
        {
            return View();
            // return _articleController.Update(id, updatedArticle);
        }

        // [HttpGet("Delete/{id}")]
        // public IActionResult DeleteView(int id)
        // { 
        //     var article = _blogContext.Articles.Find(id);
        //     if (article == null)
        //         return RedirectToAction("Index", "Home");
        //     if (article.UserId != int.Parse(User.Identity.Name))
        //         return RedirectToAction("Index", "Home");
        //     return View("~/Views/Article/Delete.cshtml", new Article { Id = id });
        // }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return View();
            // var article = _blogContext.Articles.Find(id);
            // if (article == null)
            //     return RedirectToAction("Index", "Home");
            // if (article.UserId != int.Parse(User.Identity.Name))
            //     return RedirectToAction("Index", "Home");
            // return _articleController.Delete(id);
        }   
    }
}
