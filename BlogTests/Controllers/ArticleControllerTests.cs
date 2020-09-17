using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Principal;
using BlogContext;
using BlogData.Entities;
using BlogData.ViewModels;
using BlogFakes;
using BlogMvc.Controllers;
using BlogServices;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace BlogTests.Controllers
{
    public class ArticleControllerTests
    {
        private readonly string testPassword = "lalalala1!";
        private readonly User user = new User { Id = 1, Email = "q@q.com", Password = "lalalala1!" };
        private readonly Article article = new Article { Id = 1, UserId = 1, Author = "q@q.com", Content = "test-article-content", Date = DateTime.Now, Title = "test-article-title" };
        private readonly Comment comment = new Comment { Id = 1, ArticleId = 1, UserId = 1, Date = DateTime.Now, Content = "test-comment-content", Author = "q@q.com" };
        
        private List<User> Users => new List<User> { user };
        private List<Article> Articles => new List<Article> { article };
        private List<Comment> Comments => new List<Comment> { comment };

        private IBlogDbContext fakeBlog;
        private IHttpContextAccessor httpContextAccessor;
        private DefaultHttpContext context;
        private GenericIdentity fakeIdentity;
        private GenericPrincipal principal;

        public void Setup()
        {  
            var fakeUserDbSet = new FakeUserDbSet() { data = new ObservableCollection<User>(Users) };
            var fakeArticleDbSet = new FakeArticleDbSet() { data = new ObservableCollection<Article>(Articles) };
            var fakeCommentDbSet = new FakeCommentDbSet() { data = new ObservableCollection<Comment>(Comments) };

            fakeBlog = A.Fake<IBlogDbContext>();
            httpContextAccessor = A.Fake<IHttpContextAccessor>();
            context = new DefaultHttpContext();
            fakeIdentity = A.Fake<GenericIdentity>();
            principal = A.Fake<GenericPrincipal>();

            A.CallTo(() => fakeBlog.Users).Returns(fakeUserDbSet);
            A.CallTo(() => fakeBlog.Articles).Returns(fakeArticleDbSet);
            A.CallTo(() => fakeBlog.Comments).Returns(fakeCommentDbSet);

            A.CallTo(() => principal.Identity).Returns(fakeIdentity);
            // A.CallTo(() => fakeIdentity.Name).Returns("1");
            // A.CallTo(() => context.User).Returns(principal);
            // A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);
        }

        [Test]
        public void Get_()
        {
            A.CallTo(() => fakeIdentity.Name).Returns("1");
            context.User = principal;
            A.CallTo(() => httpContextAccessor.HttpContext).Returns(context);

            var articleController = new ArticleController(fakeBlog)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContextAccessor.HttpContext }
            };

            var result = articleController.Add() as ViewResult;

            Assert.IsInstanceOf<ViewResult>(result);
        }


//   [HttpGet("Add")]
//         public IActionResult Add()
//         {
//             return View();
//         }

    }
}