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

        [HttpGet("LeaveEmployees/Show/{id}")]
        public async Task<JsonResult> GetLeaveEmployee(int id)
        {
            var result = await repository.GetLeaveEmployee(id);
            return Json(result);
        }

        [HttpGet("LeaveEmployees/Approval/{nik}")]
        public async Task<JsonResult> GetApprovalList(string nik)
        {
            var result = await repository.GetApprovalList(nik);
            return Json(result);
        }

        [HttpGet("LeaveEmployees/History/{nik}")]
        public async Task<JsonResult> GetHistoryList(string nik)
        {
            var result = await repository.GetHistoryList(nik);
            return Json(result);
        }

        [HttpGet("LeaveEmployees/getbynik/{nik}")]
        public async Task<JsonResult> GetByNik(string nik)
        {
            var result = await repository.GetByNik(nik);
            return Json(result);
        }
    }
}
