using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using System;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace BlogMvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() 
        {
            // Request  - GET
            // Response - SET

            if (Request.Cookies["Cookie.Auth"] != null && DateTime.UtcNow > DateTime.UtcNow.AddSeconds(1))
            {
                
                Response.Cookies.Delete("Cookie.Auth");
            }
                

            if (HttpContext.User.Identity.IsAuthenticated) 
                return RedirectToAction("GetById", "User", new { id = int.Parse(HttpContext.User.Identity.Name) });
            return View();
        }
    }
}
