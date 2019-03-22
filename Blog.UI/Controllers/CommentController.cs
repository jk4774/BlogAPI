using System;
﻿using Blog.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using APIController = Blog.API.Controllers;

namespace Blog.UI.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class CommentController : Controller
    {
        private readonly APIController.CommentController _commentController;
        private readonly BlogContext _blogContext;

        public CommentController(BlogContext blogContext)
        {
            _commentController = new APIController.CommentController(blogContext);
            _blogContext = blogContext;
        }

        [HttpGet("{id}")]
        public IActionResult CreateView(int id)
        {
            var article = _blogContext.Articles.Find(id);
            if (article == null)
                return RedirectToAction("Index", "Home");
            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();
            return View("~/Views/Comment/Create.cshtml", new Comment { ArticleId = id });
        }

        [HttpPost]
        public IActionResult Create([FromForm] Comment comment)
        {
            var article = _blogContext.Articles.Find(comment.ArticleId);
            if (article == null)
                return RedirectToAction("Index", "Home");

            if (string.IsNullOrWhiteSpace(comment.Content))
            {
                TempData["Message"] = "Content cannot be empty.";
                return RedirectToAction("Create", "Comment");
            }

            comment.ArticleId = comment.ArticleId;
            comment.UserId = int.Parse(User.Identity.Name);
            comment.Author = _blogContext.Users.Find(comment.UserId).Name;
            comment.Date = DateTime.Now;

            var response = _commentController.Create(comment);
            if (response.GetType() != typeof(NoContentResult))
            {
                TempData["Message"] = "Cannot add article, something went wrong.";
                return RedirectToAction("Create", "Comment");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Update/{id}")]
        public IActionResult UpdateView(int id)
        {
            var comment = _blogContext.Comments.Find(id);
            if (comment == null)
                return RedirectToAction("Index", "Home");
            if (comment.UserId != int.Parse(User.Identity.Name))
                return RedirectToAction("Index", "Home");
            return View("~/View/Comment/Update.cshtml",
                new Comment { Id = id, ArticleId = comment.ArticleId, Author = comment.Author, Content = comment.Content });
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromForm] Comment updatedComment)
        {
            return _commentController.Update(id, updatedComment);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return _commentController.Delete(id);
        }
    }
}
