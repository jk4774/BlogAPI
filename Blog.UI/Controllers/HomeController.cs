using Microsoft.AspNetCore.Mvc;

namespace Blog.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() 
        {
            if (User.Identity.IsAuthenticated)
                return Redirect("/user/" + User.Identity.Name);
            return View("~/Views/Home/Index.cshtml");
        }
    }
}
