using Blog.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Xunit;

namespace Blog.Tests
{
    public class CommentControllerTests
    {
        [Fact]
        public void GET_GetById_UserWithThisIdDoesNotExist_NotFound()
        {
            var commentController = Utils.GetCommentController();
            var Comment = commentController.GetById(9999);
            Assert.IsType<NotFoundResult>(Comment.Result);
        }

        [Fact]
        public void GET_GetById_UserWithThisIdExist_Comment()
        {
            var commentController = Utils.GetCommentController();
            commentController.Create(new Comment { Id = 1, UserId = 1, ArticleId = 1, Author = "author", Content = "content", Date = DateTime.Now });
            var Comment = commentController.GetById(1);
            Assert.IsType<ActionResult<Comment>>(Comment);
            Assert.True(Comment.Value != null);
        }

        [Fact]
        public void GET_GetByArticleId_ArticleDoesNotExist_NotFound()
        {
            var commentController = Utils.GetCommentController();
            var Comments = commentController.GetByArticleId(9999);
            Assert.IsType<NotFoundResult>(Comments.Result);
        }

        [Fact]
        public void GET_GetByArticleId_ArticleExist_ListOfComment()
        {
            var commentController = Utils.GetCommentController();
            commentController.Create(new Comment { Id = 2, UserId = 1, ArticleId = 2, Author = "author", Content = "content", Date = DateTime.Now });
            commentController.Create(new Comment { Id = 3, UserId = 1, ArticleId = 2, Author = "author", Content = "content", Date = DateTime.Now });
            var Comments = commentController.GetByArticleId(2);
            Assert.IsType<ActionResult<List<Comment>>>(Comments);
            Assert.True(Comments.Value.Count == 2);
        }
        [Fact]
        public void POST_Create_CorrectComment_CreatedAtRoute()
        {
            var commentController = Utils.GetCommentController();
            var Comment = commentController.Create(new Comment { Id = 4, UserId = 9, ArticleId = 3, Author = "author", Content = "content", Date = DateTime.Now });
            Assert.IsType<NoContentResult>(Comment);
        }

        [Fact]
        public void POST_Create_NullComment_NotFound()
        {
            var commentController = Utils.GetCommentController();
            var Comment = commentController.Create(null);
            Assert.IsType<NotFoundResult>(Comment);
        }

        [Fact]
        public void PUT_Update_UpdateExistingComment_NoContent()
        {
            var commentController = Utils.GetCommentController();
            commentController.Create(new Comment { Id = 5, UserId = 4, ArticleId = 7, Author = "auth", Content = "cont", Date = DateTime.Now });
            var Comment = commentController.Update(5, new Comment { Id = 5, UserId = 4, ArticleId = 7, Author = "author", Content = "content", Date = DateTime.Now.AddDays(1) });
            Assert.IsType<NoContentResult>(Comment);
        }

        [Fact]
        public void PUT_Update_UpdateNoExistingComment_NotFound()
        {
            var commentController = Utils.GetCommentController();
            var Comment = commentController.Update(88, new Comment { Id = 88, UserId = 87, ArticleId = 88, Author = "author", Content = "content", Date = DateTime.Now.AddDays(1) });
            Assert.IsType<NotFoundResult>(Comment);
        }

        [Fact]
        public void DELETE_Delete_DeleteExistingUser_NoContent()
        {
            var commentController = Utils.GetCommentController();
            commentController.Create(new Comment { Id = 7, UserId = 4, ArticleId = 7, Author = "author", Content = "content", Date = DateTime.Now });
            var Comment = commentController.Delete(7);
            Assert.IsType<NoContentResult>(Comment);
        }

        [Fact]
        public void DELETE_Delete_DeleteNotExistingComment_NotFound()
        {
            var commentController = Utils.GetCommentController();
            var Comment = commentController.Delete(22);
            Assert.IsType<NotFoundResult>(Comment);
        }
    }
}
