using Blog.API.Controllers;
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
            // Arrange
            var CommentController = Utils.GetCommentController();

            // Act
            var Comment = CommentController.GetById(9999);

            // Assert   
            Assert.IsType<NotFoundResult>(Comment.Result);
        }

        [Fact]
        public void GET_GetById_UserWithThisIdExist_Comment()
        {
            // Arrange
            var CommentController = Utils.GetCommentController();

            // Act
            CommentController.Create(new Comment { Id = 1, UserId = 1, ArticleId = 1, Author = "author", Content = "content", Date = DateTime.Now });
            var Comment = CommentController.GetById(1);

            // Assert
            Assert.IsType<ActionResult<Comment>>(Comment);
            Assert.True(Comment.Value != null);
        }

        [Fact]
        public void GET_GetByArticleId_ArticleDoesNotExist_NotFound()
        {
            // Arrange
            var CommentController = Utils.GetCommentController();

            // Act
            var Comments = CommentController.GetByArticleId(9999);

            // Assert
            Assert.IsType<NotFoundResult>(Comments.Result);
            //Assert.IsType<ActionResult<List<Comment>>>(Comments);
            //Assert.True(Comments.Value == null);
        }

        [Fact]
        public void GET_GetByArticleId_ArticleExist_ListOfComment()
        {
            // Arrange
            var CommentController = Utils.GetCommentController();

            // Act
            CommentController.Create(new Comment { Id = 2, UserId = 1, ArticleId = 2, Author = "author", Content = "content", Date = DateTime.Now });
            CommentController.Create(new Comment { Id = 3, UserId = 1, ArticleId = 2, Author = "author", Content = "content", Date = DateTime.Now });
            var Comments = CommentController.GetByArticleId(2);

            // Assert
            Assert.IsType<ActionResult<List<Comment>>>(Comments);
            Assert.True(Comments.Value.Count == 2);
        }
        [Fact]
        public void POST_Create_CorrectComment_CreatedAtRoute()
        {
            // Arrange
            var CommentController = Utils.GetCommentController();
            //
            // Act
            var Comment = CommentController.Create(new Comment { Id = 4, UserId = 9, ArticleId = 3, Author = "author", Content = "content", Date = DateTime.Now });

            // Assert
            Assert.IsType<NoContentResult>(Comment);
        }

        [Fact]
        public void POST_Create_NullComment_NotFound()
        {
            // Arrange
            var CommentController = Utils.GetCommentController();

            // Act
            var Comment = CommentController.Create(null);

            // Assert
            Assert.IsType<NotFoundResult>(Comment);
        }

        [Fact]
        public void PUT_Update_UpdateExistingComment_NoContent()
        {
            // Arrange
            var CommentController = Utils.GetCommentController();

            // Act
            CommentController.Create(new Comment { Id = 5, UserId = 4, ArticleId = 7, Author = "auth", Content = "cont", Date = DateTime.Now });
            var Comment = CommentController.Update(5, new Comment { Id = 5, UserId = 4, ArticleId = 7, Author = "author", Content = "content", Date = DateTime.Now.AddDays(1) });

            // Assert
            Assert.IsType<NoContentResult>(Comment);
        }

        [Fact]
        public void PUT_Update_UpdateNoExistingComment_NotFound()
        {
            // Arrange
            var CommentController = Utils.GetCommentController();

            // Act
            var Comment = CommentController.Update(88, new Comment { Id = 88, UserId = 87, ArticleId = 88, Author = "author", Content = "content", Date = DateTime.Now.AddDays(1) });

            // Assert
            Assert.IsType<NotFoundResult>(Comment);
        }

        [Fact]
        public void DELETE_Delete_DeleteExistingUser_NoContent()
        {
            // Arrange
            var CommentController = Utils.GetCommentController();

            // Act
            CommentController.Create(new Comment { Id = 7, UserId = 4, ArticleId = 7, Author = "author", Content = "content", Date = DateTime.Now });
            var Comment = CommentController.Delete(7);

            // Assert
            Assert.IsType<NoContentResult>(Comment);
        }

        [Fact]
        public void DELETE_Delete_DeleteNotExistingComment_NotFound()
        {
            // Arrange
            var CommentController = Utils.GetCommentController();

            // Act
            var Comment = CommentController.Delete(22);

            // Assert
            Assert.IsType<NotFoundResult>(Comment);
        }
    }
}
