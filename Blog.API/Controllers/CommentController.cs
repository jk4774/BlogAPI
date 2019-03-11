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
    public class CommentController : ControllerBase
    {
        private readonly BlogContext _blogContext;

        public CommentController(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        [HttpGet("{id}", Name = "GetComment")]
        public ActionResult<Comment> GetById(int id)
        {
            var comment = _blogContext.Comments.Find(id);
            if (comment == null)
                return NotFound();
            return comment;
        }

        [HttpGet("{id}", Name = "GetComments")]
        public ActionResult<List<Comment>> GetByArticleId(int id)
        {
            var comments = _blogContext.Comments.Where(x => x.ArticleId == id).ToList();
            if (comments.Count() == 0)
                return NotFound();
            return comments;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Comment comment)
        {
            if (comment == null)
                return NotFound();

            if (new[] { comment.Author, comment.Content }.Any(x => string.IsNullOrWhiteSpace(x)))
                return NotFound();

            _blogContext.Comments.Add(comment);
            _blogContext.SaveChanges();
            return CreatedAtRoute("GetComment", new { id = comment.Id }, comment);
        }

        [HttpPost("{id}")]
        public IActionResult Update(int id, [FromBody] Comment updatedComment)
        {
            var comment = _blogContext.Comments.Find(id);
            if (new[] { comment, updatedComment }.Any(x => x == null))
                return NotFound();

            if (new[] { updatedComment.Author, updatedComment.Content }.Any(x => string.IsNullOrWhiteSpace(x)) || updatedComment.Date == null)
                return NotFound();

            comment.UserId = updatedComment.UserId;
            comment.ArticleId = updatedComment.ArticleId;
            comment.Author = updatedComment.Author;
            comment.Content = updatedComment.Content;
            comment.Date = DateTime.Now;
            _blogContext.Comments.Update(comment);
            _blogContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var comment = _blogContext.Comments.Find(id);
            if (comment == null)
                return NotFound();

            _blogContext.Comments.Remove(comment);
            _blogContext.SaveChanges();
            return NoContent();
        }
    }
}