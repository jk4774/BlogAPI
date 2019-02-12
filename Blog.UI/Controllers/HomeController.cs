using Microsoft.AspNetCore.Mvc;

namespace Blog.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return Redirect("/user/" + HttpContext.User.Identity.Name);
            return View("~/Views/Home/Index.cshtml");
        }
    }
}
