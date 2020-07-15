using Microsoft.AspNetCore.Mvc;

namespace BlogMvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() 
        {
            if (HttpContext.User.Identity.IsAuthenticated) 
                return RedirectToAction("GetById", "User", new { id = int.Parse(HttpContext.User.Identity.Name) });
            return View();
        }
    }
}
