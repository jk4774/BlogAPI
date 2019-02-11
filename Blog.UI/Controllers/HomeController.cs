using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Blog.UI.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        [Route("Index")]
        [Route("~/")]
        public IActionResult Index()
        {
            //return RedirectToAction()
            return Ok("qwer");
            //return Ok("qwer");
            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    return Redirect("/user/1");
            //}
            //return Redirect("~/Index.cshtml");
        }
    }
}
