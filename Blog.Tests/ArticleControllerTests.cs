using Xunit;
using Blog.API.Controllers;
using Blog.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Blog.Tests
{
    public class ArticleControllerTests
    {
        private readonly ArticleController _articleController;
        public ArticleControllerTests()
        {
            _articleController = Utils.GetArticleController();
        }

        [Fact]
        public void GET_GetAll_NoArticles_NotFound()
        {
            // Act

            _articleController.DeleteArticles();
            var Articles = _articleController.GetAll();

            // Assert
            Assert.IsType<NotFoundResult>(Articles.Result);
        }

        [Fact]
        public void GET_GetAll_ArticlesCoundIsZero_DictionaryArticleAndListOfCommentsHasOneElements()
        {
            // Arrange
            var ArticleController = Utils.GetArticleController();

            // Act
            _articleController.DeleteArticles();
            _articleController.Create(new Article { Id = 1, UserId = 1, UserName = "authorName", Title = "Test1", Content = "Content2", Date = DateTime.Now });
            var Articles = _articleController.GetAll();

            // Assert
            Assert.IsType<Dictionary<Article, List<Comment>>>(Articles.Value);
            Assert.True(Articles.Value != null);
            Assert.True(Articles.Value.Count == 1);
        }

        [Fact]
        public void GET_GetById_ArticleWithThisIdExist_TupleArticleAndComments()
        {
            // Arrange
            var ArticleController = Utils.GetArticleController();

            // Act
            _articleController.Create(new Article { Id = 2, UserId = 1, UserName = "authorName", Title = "title", Content = "content", Date = DateTime.Now });
            var Article = _articleController.GetById(2);

            // Assert
            Assert.IsType<ActionResult<Tuple<Article, List<Comment>>>>(Article);
            Assert.True(Article.Value != null);
        }

        [Fact]
        public void GET_GetById_ArticleWithThisIdDoesNotExist_NotFound()
        {
            // Act
            var Article = _articleController.GetById(777);

            // Assert
            Assert.IsType<NotFoundResult>(Article.Result);
        }

        [Fact]
        public void POST_Create_TitleIsWhitespace_NoContent()
        {
            // Act
            var Article = _articleController.Create(new Article { Id = 3, UserId = 1, UserName = "authorName", Title = " ", Content = "con", Date = DateTime.Now });

            // Assert
            Assert.IsType<NotFoundResult>(Article);
        }

        [Fact]
        public void POST_Create_ContentIsEmpty_NotFound()
        {
            // Act

            var Article = _articleController.Create(new Article { Id = 3, UserId = 1, UserName = "authorName", Title = "tit", Content = " ", Date = DateTime.Now });

            // Assert
            Assert.IsType<NotFoundResult>(Article);
        }

        [Fact]
        public void POST_Create_CorrectArticle_NoContent()
        {
            // Act
            var Article = _articleController.Create(new Article { Id = 3, UserId = 1, UserName = "authorName", Title = "title", Content = "content", Date = DateTime.Now });

            // Assert
            Assert.IsType<NoContentResult>(Article);
        }

        [Fact]
        public void POST_Create_IncorrectArticle_NotFound()
        {
            // Act
            var Article = _articleController.Create(null);

            // Assert
            Assert.IsType<NotFoundResult>(Article);
        }


        [Fact]
        public void PUT_Update_UpdateExistingArticle_NoContent()
        {
            // Act
            _articleController.Create(new Article { Id = 4, UserId = 2, UserName = "authorName", Title = "title", Content = "content", Date = DateTime.Now });
            var Article = _articleController.Update(4, new Article { Id = 4, UserId = 1, UserName = "authorName", Title = "title2", Content = "newContent", Date = DateTime.Now.AddDays(1) });

            // Assert
            Assert.IsType<NoContentResult>(Article);
        }

        [Fact]
        public void PUT_Update_UpdateNoExistingArticle_NotFound()
        {
            // Act
            var Article = _articleController.Update(5, new Article { Id = 5, UserId = 1, UserName = "authorName", Title = "title", Content = "content", Date = DateTime.Now });

            // Assert
            Assert.IsType<NotFoundResult>(Article);
        }

        [Fact]
        public void DELETE_Delete_DeleteExistingArticle_NoContent()
        {
            // Act
            _articleController.Create(new Article { Id = 6, UserId = 1, UserName = "authorName", Title = "title", Content = "content", Date = DateTime.Now });
            var Article = _articleController.Delete(6);

            // Assert
            Assert.IsType<NoContentResult>(Article);
        }

        [Fact]
        public void DELETE_Delete_DeleteNotExistingArticle_NotFound()
        {
            // Act  
            var Article = _articleController.Delete(77);

            // Assert
            Assert.IsType<NotFoundResult>(Article);
        }

        [Fact]
        public void DELETE_Delete_DeleteExistingArticleWithComments_NoContent()
        {
            //Arrange
            var commentController = Utils.GetCommentController();

            // Act
            _articleController.Create(new Article { Id = 77, UserId = 2, UserName = "authorName", Title = "title", Content = "content35", Date = DateTime.Now });
            commentController.Create(new Comment { Id = 123, UserId = 3, ArticleId = 77, Author = "authorName", Content = "COMMENTABOUTASDF", Date = DateTime.Now });
            commentController.Create(new Comment { Id = 124, UserId = 4, ArticleId = 77, Author = "authorName", Content = "COMMENTABOUTASDF2", Date = DateTime.Now });
            var Article = _articleController.Delete(77);

            // Assert
            Assert.IsType<NoContentResult>(Article);
        }

    }
}
