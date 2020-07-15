// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
// using BlogMvc.Models;
// using Microsoft.AspNetCore.Authorization;
// using BlogContext;
// using BlogEntities;

// namespace BlogMvc.Controllers
// {
//     [Authorize]
//     [Route("[controller]")]
//     public class CommentController : Controller
//     {
//         private readonly Blog _blog; 
//         public CommentController(Blog blog)
//         {
//             _blog = blog;
//         }

//         [HttpGet("{id?}")]
//         public IActionResult CreateView(int id)
//         {
//             // var article = _blogContext.Articles.Find(id);
//             // if (article == null)
//             //     return RedirectToAction("Index", "Home");
//             // if (TempData["Message"] != null)
//             //     ViewBag.Message = TempData["Message"].ToString();
//             // return View("~/Views/Comment/Create.cshtml", new Comment { ArticleId = id });
//             return View();
//         }

//         [HttpPost]
//         public IActionResult Create([FromForm] Comment comment)
//         {
//             // var article = _blogContext.Articles.Find(comment.ArticleId);
//             // if (article == null)
//             //     return RedirectToAction("Index", "Home");

//             // if (string.IsNullOrWhiteSpace(comment.Content))
//             // {
//             //     TempData["Message"] = "Content cannot be empty.";
//             //     return RedirectToAction("Create", "Comment", new { id = comment.ArticleId });
//             // }

//             // comment.UserId = int.Parse(User.Identity.Name);
//             // comment.Author = _blogContext.Users.Find(comment.UserId).Name;
//             // comment.Date = DateTime.Now;

//             // var response = _commentController.Create(comment);
//             // if (response.GetType() != typeof(NoContentResult))
//             // {
//             //     TempData["Message"] = "Cannot add article, something went wrong.";
//             //     return RedirectToAction("Create", "Comment", new { id = comment.ArticleId });
//             // }

//             // return RedirectToAction("Index", "Home");
//             return View();
//         }

//         /*[HttpGet("Update/{id}")]
//         public IActionResult UpdateView(int id)
//         {
//             var comment = _blogContext.Comments.Find(id);
//             if (comment == null)
//                 return RedirectToAction("Index", "Home");
//             if (comment.UserId != int.Parse(User.Identity.Name))
//                 return RedirectToAction("Index", "Home");
//             return View("~/View/Comment/Update.cshtml",
//                 new Comment { Id = id, ArticleId = comment.ArticleId, Author = comment.Author, Content = comment.Content });
//             return View();
//         }

//         [HttpPut("Update/{id}")]
//         public IActionResult Update(int id, [FromForm] Comment updatedComment)
//         {
//             return _commentController.Update(id, updatedComment);
//             return View();
//         }*/

//         [HttpDelete("{id}")]
//         public IActionResult Delete(int id)
//         {
//             // return _commentController.Delete(id);
//             return View();
//         }
        
//     }
// }

// // HELPERS>cs
// //  public static void DeleteCookie(HttpContext httpContext, string cookieName = "access_token")
// //         {
// //             if (httpContext.Request.Cookies[cookieName] != null)
// //             {
// //                 try
// //                 {
// //                     httpContext.Response.Cookies.Delete(cookieName);
// //                 }
// //                 catch
// //                 {
// //                     throw new Exception(string.Format("Something went wrong. Cannot delete cookie with name: {0}", cookieName));
// //                 }
// //             }
// //         }
// //     }