using API.Models;
using Client.Base;
using Client.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class LeavesController : BaseController<Leave, LeaveRepository, string>
    {
        private readonly LeaveRepository leaveRepository;
        public LeavesController(LeaveRepository repository) : base(repository)
        {
            this.leaveRepository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Request()
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
        public IActionResult History()
        {
            return View();
        }
        public IActionResult Quota()
        {
            return View();
        }
    }
}
