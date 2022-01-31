using API.Models;
using API.ViewModel;
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

        [HttpGet]
        public async Task<JsonResult> Normal()
        {
            var result = await repository.GetNormalLeave();
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> Special()
        {
            var result = await repository.GetSpecialLeave();
            return Json(result);
        }

        [HttpPost]
        public JsonResult LeaveRequest(LeaveVM leaveRequest)
        {
            var result = repository.LeaveRequest(leaveRequest);
            return Json(result);
        }

        [HttpPut]
        public JsonResult Approval(LeaveVM leaveApproval)
        {
            var result = repository.LeaveApproval(leaveApproval);
            return Json(result);
        }

    }
}
