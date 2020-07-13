using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BlogMvc.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("GetUser", new { id = int.Parse(HttpContext.User.Identity.Name) });
            }
            else
            {
                return View();
            }
        }
    }
}
