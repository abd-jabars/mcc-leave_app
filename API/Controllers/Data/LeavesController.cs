﻿using API.Models;
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
    public class LeavesController : BaseController<Leave, LeaveRepository, int>
    {
        public LeavesController(LeaveRepository leaveRepository) : base(leaveRepository)
        {

        }
    }
}
