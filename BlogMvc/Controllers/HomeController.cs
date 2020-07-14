using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BlogMvc.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index() 
        {
            if (HttpContext.User.Identity.IsAuthenticated) 
                return await Task.Run(() => RedirectToAction("GetUser", new { id = int.Parse(HttpContext.User.Identity.Name) }));
            return await Task.Run(() => View());
        }
    }
}
