using Xunit;
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
            var articleController = Utils.GetArticleController();
            articleController.DeleteArticles();
            var Articles = articleController.GetAll();
            Assert.IsType<NotFoundResult>(Articles.Result);
        }

        [Fact]
        public void GET_GetAll_ArticlesCoundIsZero_DictionaryArticleAndListOfCommentsHasOneElements()
        {
            var articleController = Utils.GetArticleController();
            articleController.DeleteArticles();
            articleController.Create(new Article { Id = 1, UserId = 1, UserName = "authorName", Title = "Test1", Content = "Content2", Date = DateTime.Now });
            var Articles = articleController.GetAll();
            Assert.IsType<Dictionary<Article, List<Comment>>>(Articles.Value);
            Assert.True(Articles.Value != null);
            Assert.True(Articles.Value.Count == 1);
        }

        [Fact]
        public void GET_GetById_ArticleWithThisIdExist_TupleArticleAndComments()
        {
            var articleController = Utils.GetArticleController();
            articleController.Create(new Article { Id = 1, UserId = 1, UserName = "authorName", Title = "title", Content = "content", Date = DateTime.Now });
            var Article = articleController.GetById(1);
            Assert.IsType<ActionResult<Tuple<Article, List<Comment>>>>(Article);
            Assert.True(Article.Value != null);
        }

        [Fact]
        public void GET_GetById_ArticleWithThisIdDoesNotExist_NotFound()
        {
            var articleController = Utils.GetArticleController();
            var Article = articleController.GetById(1);
            Assert.IsType<NotFoundResult>(Article.Result);
        }

        [Fact]
        public void POST_Create_TitleIsWhitespace_NoContent()
        {
            var articleController = Utils.GetArticleController();
            var Article = articleController.Create(new Article { Id = 1, UserId = 1, UserName = "authorName", Title = " ", Content = "con", Date = DateTime.Now });
            Assert.IsType<NotFoundResult>(Article);
        }

        [Fact]
        public void POST_Create_ContentIsEmpty_NotFound()
        {
            var articleController = Utils.GetArticleController();
            var Article = articleController.Create(new Article { Id = 1, UserId = 1, UserName = "authorName", Title = "tit", Content = " ", Date = DateTime.Now });
            Assert.IsType<NotFoundResult>(Article);
        }

        [Fact]
        public void POST_Create_CorrectArticle_NoContent()
        {
            var articleController = Utils.GetArticleController();
            var Article = articleController.Create(new Article { Id = 1, UserId = 1, UserName = "authorName", Title = "title", Content = "content", Date = DateTime.Now });
            Assert.IsType<NoContentResult>(Article);
        }

        [Fact]
        public void POST_Create_IncorrectArticle_NotFound()
        {
            var articleController = Utils.GetArticleController();
            var Article = articleController.Create(null);
            Assert.IsType<NotFoundResult>(Article);
        }


        [Fact]
        public void PUT_Update_UpdateExistingArticle_NoContent()
        {
            var articleController = Utils.GetArticleController();
            articleController.Create(new Article { Id = 1, UserId = 2, UserName = "authorName", Title = "title", Content = "content", Date = DateTime.Now });
            var Article = articleController.Update(1, new Article { Id = 1, UserId = 1, UserName = "authorName", Title = "title2", Content = "newContent", Date = DateTime.Now.AddDays(1) });
            Assert.IsType<NoContentResult>(Article);
        }

        [Fact]
        public void PUT_Update_UpdateNoExistingArticle_NotFound()
        {
            var articleController = Utils.GetArticleController();
            var Article = articleController.Update(1, new Article { Id = 1, UserId = 1, UserName = "authorName", Title = "title", Content = "content", Date = DateTime.Now });
            Assert.IsType<NotFoundResult>(Article);
        }

        [Fact]
        public void DELETE_Delete_DeleteExistingArticle_NoContent()
        {
            var articleController = Utils.GetArticleController();
            articleController.Create(new Article { Id = 1, UserId = 1, UserName = "authorName", Title = "title", Content = "content", Date = DateTime.Now });
            var Article = articleController.Delete(1);
            Assert.IsType<NoContentResult>(Article);
        }

        [Fact]
        public void DELETE_Delete_DeleteNotExistingArticle_NotFound()
        {
            var articleController = Utils.GetArticleController();
            var Article = articleController.Delete(1);
            Assert.IsType<NotFoundResult>(Article);
        }
    }
}
