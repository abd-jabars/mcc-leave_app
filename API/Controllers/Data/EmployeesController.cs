using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : BaseController<Employee, EmployeeRepository, string>
    {
        private readonly EmployeeRepository employeeRepository;
        public EmployeesController(EmployeeRepository employeeRepository) : base(employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpPost]
        [Route("Register")]
        public ActionResult Register(Register register)
        {
            try
            {
                var result = employeeRepository.Register(register);
                if (result == 1)
                    return Ok(new { status = HttpStatusCode.BadRequest, result = result, message = "Email and phone already used" });
                else if (result == 2)
                    return Ok(new { status = HttpStatusCode.BadRequest, result = result, message = "Email already used" });
                else if (result == 3)
                    return Ok(new { status = HttpStatusCode.BadRequest, result = result, message = "Phone already used" });
                else
                    return Ok(new { status = HttpStatusCode.OK, result = result, message = "Data inserted" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = ex.ToString() });
            }
        }

        [HttpGet]
        [Route("RegisteredData")]
        public ActionResult<Register> GetRegisterVM()
        {
            try
            {
                var result = employeeRepository.GetRegisteredData();
                if (result.Count() <= 0)
                    return NotFound(result);
                else
                    return Ok(result);
                //return Ok(new ReturnMessage(HttpStatusCode.OK, result, "Data found"));
            }
            catch (Exception ex)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = ex.ToString() });
            }
        }

        [HttpGet]
        [Route("RegisteredData/{NIK}")]
        public ActionResult<Register> GetRegisteredData(string NIK)
        {
            try
            {
                var result = employeeRepository.GetRegisteredData(NIK);
                if (result == null)
                    return NotFound(result);
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = ex.ToString() });
            }
        }

        [HttpPut]
        [Route("UpdateRegistered")]
        public ActionResult UpdateRegisteredData(Register register)
        {
            try
            {
                var result = employeeRepository.UpdateRegisteredData(register);
                if (result == 1)
                    return Ok(new { status = HttpStatusCode.BadRequest, result = result, message = "Phone already used" });
                else if (result == 2)
                    return Ok(new { status = HttpStatusCode.BadRequest, result = result, message = "Email already used" });
                else if (result == 3)
                    return Ok(new { status = HttpStatusCode.BadRequest, result = result, message = "Phone and email already used" });
                else
                    return Ok(new { status = HttpStatusCode.OK, result = result, message = "Data updated" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = ex.ToString() });
            }
        }

        [HttpGet("TestCORS")]
        public ActionResult TestCORS()
        {
            return Ok("Test CORS berhasil");
        }

    }
}
