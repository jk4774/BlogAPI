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
                var expires = DateTime.Parse(Request.Cookies["expires"]);

                if (DateTime.UtcNow.AddMinutes(3) > expires){

                }

                // return Ok ("Curre: " + DateTime.UtcNow.AddMinutes(3).ToLongTimeString()
                //  + "   FROM: " + token.ValidFrom + 
                //  "   TO:" + token.ValidTo + "\n"  + 
                //  (DateTime.UtcNow.AddMinutes(3) > token.ValidTo));
                
                //if (DateTime.UtcNow.AddMinutes(3) > token.ValidTo) 
                //    Response.Cookies.Delete("access_token");
            }

            if (User.Identity.IsAuthenticated)
                return Redirect("/user/" + User.Identity.Name);
            return View("~/Views/Home/Index.cshtml");
        }
    }
}
