using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BlogMvc.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index() 
        {
            if (Request.Cookies["auth_cookie"] != null)
            {
                var exp = User.Claims.FirstOrDefault(x=>x.Type.Equals("Expiration"))?.Value;
                if (DateTime.UtcNow > DateTime.Parse(exp??DateTime.UtcNow.AddSeconds(1).ToString()))
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            if (HttpContext.User.Identity.IsAuthenticated) 
                return RedirectToAction("GetById", "User", new { id = int.Parse(User.Identity.Name) });
            return View();
        }
    }
}
