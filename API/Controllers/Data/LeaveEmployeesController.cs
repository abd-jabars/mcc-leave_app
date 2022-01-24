using API.Models;
using API.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveEmployeesController : BaseController<LeaveEmployee, LeaveEmployeeRepository, int>
    {
        public LeaveEmployeesController(LeaveEmployeeRepository leaveEmployeeRepository) : base(leaveEmployeeRepository)
        {

        }
    }
}
