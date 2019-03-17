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
        [Fact]
        public void GET_GetAll_NoArticles_NotFound()
        {
            // Arrange
            var ArticleController = Utils.GetArticleController();

            // Act
            ArticleController.DeleteArticles();
            var Articles = ArticleController.GetAll();

            // Assert
            Assert.IsType<NotFoundResult>(Articles.Result);
        }

        [Fact]
        public void GET_GetAll_ArticlesCoundIsZero_DictionaryArticleAndListOfCommentsHasOneElements()
        {
            // Arrange
            var ArticleController = Utils.GetArticleController();

            // Act
            ArticleController.DeleteArticles();
            ArticleController.Create(new Article { Id = 1, Author = "authorName", Title = "Test1", Content = "Content2", Date = DateTime.Now });
            var Articles = ArticleController.GetAll(); 

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
            ArticleController.Create(new Article { Id = 2, Author = "authorName", Title = "title", Content = "content", Date = DateTime.Now });
            var Article = ArticleController.GetById(2);

            // Assert
            Assert.IsType<ActionResult<Tuple<Article, List<Comment>>>>(Article);
            Assert.True(Article.Value != null);
        }

        [Fact]
        public void GET_GetById_ArticleWithThisIdDoesNotExist_NotFound()
        {
            // Arrange 
            var ArticleController = Utils.GetArticleController();

            // Act
            var Article = ArticleController.GetById(777);

            // Assert
            Assert.IsType<NotFoundResult>(Article.Result);
        }

#region new2
        [Fact]
        public void POST_Create_TitleIsWhitespace_NoContent()
        {
            // Arrange
            var ArticleController = Utils.GetArticleController();

            // Act
            var Article = ArticleController.Create(new Article { Id = 3, Author = "authorName", Title = " ", Content = "con", Date = DateTime.Now });

            // Assert
            Assert.IsType<NotFoundResult>(Article);
        }

        [Fact]
        public void POST_Create_ContentIsEmpty_NotFound() 
        {
              // Arrange
            var ArticleController = Utils.GetArticleController();

            // Act
            var Article = ArticleController.Create(new Article { Id = 3, Author = "authorName", Title = "tit", Content = " ", Date = DateTime.Now });

            // Assert
            Assert.IsType<NotFoundResult>(Article);
        }
#endregion


        [Fact]
        public void POST_Create_CorrectArticle_NoContent()
        {
            // Arrange 
            var ArticleController = Utils.GetArticleController();

            // Act
            var Article = ArticleController.Create(new Article { Id = 3, Author = "authorName", Title = "title", Content = "content", Date = DateTime.Now });

            // Assert
            Assert.IsType<NoContentResult>(Article);
        }

        [Fact]
        public void POST_Create_IncorrectArticle_NotFound()
        {
            // Arrange 
            var ArticleController = Utils.GetArticleController();

            // Act
            var Article = ArticleController.Create(null);

            // Assert
            Assert.IsType<NotFoundResult>(Article);
        }


        [Fact]
        public void PUT_Update_UpdateExistingArticle_NoContent()
        {
            // Arrange 
            var ArticleController = Utils.GetArticleController();

            // Act
            ArticleController.Create(new Article { Id = 4, Author = "authorName", Title = "title", Content = "content", Date = DateTime.Now });
            var Article = ArticleController.Update(4, new Article { Id = 4, Author = "authorName", Title = "title2", Content = "newContent", Date = DateTime.Now.AddDays(1) });

            // Assert
            Assert.IsType<NoContentResult>(Article);
        }

        [Fact]
        public void PUT_Update_UpdateNoExistingArticle_NotFound()
        {
            // Arrange 
            var ArticleController = Utils.GetArticleController();

            // Act
            var Article = ArticleController.Update(5, new Article { Id = 5, Author = "authorName", Title = "title", Content = "content", Date = DateTime.Now });

            // Assert
            Assert.IsType<NotFoundResult>(Article);
        }

        [Fact]
        public void DELETE_Delete_DeleteExistingArticle_NoContent()
        {
            // Arrange 
            var ArticleController = Utils.GetArticleController();

            // Act
            ArticleController.Create(new Article { Id = 6, Author = "authorName", Title = "title", Content = "content", Date = DateTime.Now });
            var Article = ArticleController.Delete(6);

            // Assert
            Assert.IsType<NoContentResult>(Article);
        }

        [Fact]
        public void DELETE_Delete_DeleteNotExistingArticle_NotFound()
        {
            // Arrange 
            var ArticleController = Utils.GetArticleController();

            // Act
            var Article = ArticleController.Delete(77);

            // Assert
            Assert.IsType<NotFoundResult>(Article);
        }
    }
}
