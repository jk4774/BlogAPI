using Blog.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using APIController = Blog.API.Controllers;

namespace Blog.UI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : Controller
    {
        private readonly APIController.CommentController _commentController;

        public CommentController(BlogContext blogContext)
        {
            _commentController = new APIController.CommentController(blogContext);
        }

        [HttpGet("{id}", Name = "GetComment")]
        public ActionResult<Comment> GetById(int id)
        {
            return _commentController.GetById(id);
        }

        [HttpGet("{id}", Name = "GetComments")]
        public ActionResult<List<Comment>> GetByArticleId(int id)
        {
            return _commentController.GetByArticleId(id);
        }

        [HttpPost]
        public IActionResult Create([FromForm] Comment comment)
        {
            return _commentController.Create(comment);
        }

        [HttpPost("{id}")]
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