using API.Models;
using API.ViewModel;
using Client.Base;
using Client.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class EmployeesController : BaseController<Employee, EmployeeRepository, string>
    {
        private readonly EmployeeRepository employeeRepository;
        public EmployeesController(EmployeeRepository repository) : base(repository)
        {
            this.employeeRepository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult DataTable()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Register(RegisterVM register)
        {
            var result = repository.Register(register);
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> RegisteredData()
        {
            var result = await repository.RegisteredData();
            return Json(result);
        }

        [HttpGet("Employees/RegisteredData/{NIK}")]
        public async Task<JsonResult> GetRegisteredById(string NIK)
        {
            var result = await repository.RegisteredData(NIK);
            return Json(result);
        }

        [HttpPut]
        public JsonResult UpdateRegisteredData(RegisterVM register)
        {
            var result = repository.UpdateRegisteredData(register);
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetUserData()
        {
            var NIK = HttpContext.Session.GetString("userNik");
            var result = await repository.RegisteredData(NIK);
            return Json(result);
        }

    }
}
