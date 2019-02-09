using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Blog.UI.Controllers
{
    public class HomeController : Controller
    {
        [Route("Index")]
        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/user/1");
            }
            return Redirect("~/Index.cshtml");
        }
    }
}
