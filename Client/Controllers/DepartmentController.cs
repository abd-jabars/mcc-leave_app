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
    public class DepartmentController : BaseController<Department, DepartmentRepository, string>
    {
        private readonly DepartmentRepository departmentRepository;
        public DepartmentController(DepartmentRepository repository) : base(repository)
        {
            this.departmentRepository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
