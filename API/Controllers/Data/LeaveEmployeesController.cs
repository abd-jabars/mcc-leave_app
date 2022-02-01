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
        private readonly LeaveEmployeeRepository leaveEmployeeRepository;
        public LeaveEmployeesController(LeaveEmployeeRepository leaveEmployeeRepository) : base(leaveEmployeeRepository)
        {
            this.leaveEmployeeRepository = leaveEmployeeRepository;
        }

        [HttpGet("show/{id}")]
        public ActionResult GetRegisteredData(int id)
        {
            try
            {
                var code = 0;
                var message = "";
                //var result = ();
                var count = leaveEmployeeRepository.GetLeaveEmployee(id).Count();
                var result = leaveEmployeeRepository.GetLeaveEmployee(id);

                if (count > 0)
                {
                    code = StatusCodes.Status200OK;
                    //message = "";
                }
                else
                {
                    //code = Response.StatusCode;
                    code = StatusCodes.Status400BadRequest;
                    message = "Table content is empty";
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok($"{e.Message}");
            }
        }

        [HttpGet("Approval/{nik}")]
        public ActionResult GetApprovalList(string nik)
        {
            try
            {
                var code = 0;
                var message = "";
                //var result = ();
                var count = leaveEmployeeRepository.GetApprovalList(nik).Count();
                var result = leaveEmployeeRepository.GetApprovalList(nik);

                if (count > 0)
                {
                    code = StatusCodes.Status200OK;
                    //message = "";
                }
                else
                {
                    //code = Response.StatusCode;
                    code = StatusCodes.Status400BadRequest;
                    message = "Table content is empty";
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok($"{e.Message}");
            }
        }

        [HttpGet("History/{nik}")]
        public ActionResult getHistoryList(string nik)
        {
            try
            {
                var code = 0;
                var message = "";
                //var result = ();
                var count = leaveEmployeeRepository.GetHistoryList(nik).Count();
                var result = leaveEmployeeRepository.GetHistoryList(nik);

                if (count > 0)
                {
                    code = StatusCodes.Status200OK;
                    //message = "";
                }
                else
                {
                    //code = Response.StatusCode;
                    code = StatusCodes.Status400BadRequest;
                    message = "Table content is empty";
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok($"{e.Message}");
            }
        }

        [HttpGet("OnLeave/{nik}")]
        public ActionResult GetonLeaveList(string nik)
        {
            try
            {
                var code = 0;
                var message = "";
                //var result = ();
                var count = leaveEmployeeRepository.GetonLeaveList(nik).Count();
                var result = leaveEmployeeRepository.GetonLeaveList(nik);

                if (count > 0)
                {
                    code = StatusCodes.Status200OK;
                    //message = "";
                }
                else
                {
                    //code = Response.StatusCode;
                    code = StatusCodes.Status400BadRequest;
                    message = "Table content is empty";
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok($"{e.Message}");
            }
        }

        [HttpGet("GetLeave/{nik}")]
        public ActionResult GetLeave(string nik)
        {
            try
            {
                var code = 0;
                var message = "";
                //var result = ();
                var count = leaveEmployeeRepository.GetLeave(nik).Count();
                var result = leaveEmployeeRepository.GetLeave(nik);

                if (count > 0)
                {
                    code = StatusCodes.Status200OK;
                    //message = "";
                }
                else
                {
                    //code = Response.StatusCode;
                    code = StatusCodes.Status400BadRequest;
                    message = "Table content is empty";
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok($"{e.Message}");
            }
        }
    }
}
