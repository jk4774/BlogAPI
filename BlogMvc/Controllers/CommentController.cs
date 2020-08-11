// using Microsoft.AspNetCore.Mvc;
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

//         [HttpGet("Add/{id}")]
//         public IActionResult Add(int id)
//         {
//             return View();
//         }

//         [HttpPost("Add/{id}")]
//         public async Task<IActionResult> Add(int id, [FromForm] Comment comment)
//         {
//             var article = await _blog.Articles.FindAsync(id);
//             if (article == null)
//                 return NotFound();
            
//             comment

//         }

//         // Add
//         // Update
//         // Delete


//     }
// }
