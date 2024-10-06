using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
