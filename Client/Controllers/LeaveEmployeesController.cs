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
    }
}
