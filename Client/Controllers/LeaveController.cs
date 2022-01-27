using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class LeaveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Approval()
        {
            return View();
        }
        public IActionResult Calendar()
        {
            return View();
        }
    }
}
