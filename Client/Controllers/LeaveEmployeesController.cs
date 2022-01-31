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
    public class LeaveEmployeesController : BaseController<LeaveEmployee, LeaveEmployeeRepository, int>
    {
        private readonly LeaveEmployeeRepository leaveEmployeeRepository;

        public LeaveEmployeesController(LeaveEmployeeRepository repository) : base(repository)
        {
            this.leaveEmployeeRepository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetByManager(string nik)
        {
            var result = await repository.GetByManager(nik);
            return Json(result);
        }

        [HttpGet("LeaveEmployees/Show/{id}")]
        public async Task<JsonResult> GetLeaveEmployee(int id)
        {
            var result = await repository.GetLeaveEmployee(id);
            return Json(result);
        }

        [HttpGet("LeaveEmployees/Approval")]
        public async Task<JsonResult> GetApprovalList()
        {
            var result = await repository.GetApprovalList();
            return Json(result);
        }

        [HttpGet("LeaveEmployees/History")]
        public async Task<JsonResult> GetHistoryList()
        {
            var result = await repository.GetHistoryList();
            return Json(result);
        }

    }
}
