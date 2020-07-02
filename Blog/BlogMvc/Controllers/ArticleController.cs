using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlogMvc.Models;
using Microsoft.AspNetCore.Authorization;

namespace BlogMvc.Controllers
{
    [Authorize]
    public class ArticleController : Controller
    {
        
    }
}
