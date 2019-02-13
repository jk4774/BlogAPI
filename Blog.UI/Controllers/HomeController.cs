using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return Redirect("/user/");
            return View("~/Views/Home/Index.cshtml");
        }
    }
}
