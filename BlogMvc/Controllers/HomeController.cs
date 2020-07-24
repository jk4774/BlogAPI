using Microsoft.AspNetCore.Mvc;

namespace BlogMvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() 
        {
            if (Request.Cookies["auth_cookie"] != null && !User.Identity.IsAuthenticated)
                Response.Cookies.Delete("auth_cookie");

            if (User.Identity.IsAuthenticated)
                return RedirectToAction("GetById", "User", new { id = User.Identity.Name });
            return View();
        }
    }
}
