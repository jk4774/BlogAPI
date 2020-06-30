using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Blog.Mvc.Models;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Mvc.Controllers
{
    [Authorize]
    public class ArticleController : Controller
    {
        
    }
}
