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

    }
}
