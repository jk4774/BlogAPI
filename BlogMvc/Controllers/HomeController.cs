using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace BlogMvc.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index() 
        {
            if (Request.Cookies["auth_cookie"] != null && !User.Identity.IsAuthenticated) 
            {
                Response.Cookies.Delete("auth_cookie");
            }
                // await HttpContext.SignOutAsync();

            if (User.Identity.IsAuthenticated) 
                return RedirectToAction("GetById", "User", new { id = User.Identity.Name });
            return View();
        }
    }
}
