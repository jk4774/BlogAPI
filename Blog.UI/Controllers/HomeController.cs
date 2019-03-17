using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Blog.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() 
        {   
            if (Request.Cookies["access_token"] != null) 
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var token = jwtHandler.ReadJwtToken(Request.Cookies["access_token"]);
                
                if (DateTime.UtcNow > token.ValidTo)
                    Response.Cookies.Delete("access_token");
            }

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
                // ViewBag.IsDanger = TempData["IsDanger"];
            }

            if (User.Identity.IsAuthenticated)
                return Redirect("/user/" + User.Identity.Name);
            return View("~/Views/Home/Index.cshtml");
        }
    }
}
