using System;
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
            if (Request.Cookies["auth_cookie"] != null) 
            {
                var auth = await HttpContext.AuthenticateAsync();  
                if (auth?.Properties != null && DateTime.UtcNow > auth.Properties.ExpiresUtc)
                    await HttpContext.SignOutAsync();
            }

            if (HttpContext.User.Identity.IsAuthenticated) 
                return RedirectToAction("GetById", "User", new { id = User.Identity.Name });
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}
