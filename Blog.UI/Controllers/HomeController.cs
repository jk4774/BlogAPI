using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Blog.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() 
        {
<<<<<<< HEAD
            // if (Response.StatusCode == 302) {
            //     Response.Cookies.Delete("access_token");
            // }
=======
>>>>>>> 985bfe7c4ee18457b422a6e3096a96a463e2690f
            if (Request.Cookies["access_token"] != null) 
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var token = jwtHandler.ReadJwtToken(Request.Cookies["access_token"]);
                
                if (DateTime.UtcNow > token.ValidTo)
                    Response.Cookies.Delete("access_token");
            }

            if (User.Identity.IsAuthenticated)
                return Redirect("/user/" + User.Identity.Name);
            return View("~/Views/Home/Index.cshtml");
        }
    }
}
