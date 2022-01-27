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
    public class LeavesController : BaseController<Models.Leave, LeaveRepository, int>
    {
        private readonly LeaveRepository leaveRepository;
        public LeavesController(LeaveRepository leaveRepository) : base(leaveRepository)
        {
            this.leaveRepository = leaveRepository;
        }

        [HttpPost]
        [Route("Request")]
        public ActionResult LeaveRequest(LeaveVM leaveRequest)
        {
            var result = leaveRepository.LeaveRequest(leaveRequest);
            if (result == 1)
            {
                return Ok(new { status = HttpStatusCode.NotFound, result = result, message = "Data employee tidak ditemukan" });

            }
            else if (result == 2)
            {
                return Ok(new { status = HttpStatusCode.NotFound, result = result, message = "Data cuti tidak ditemukan" });
            }
            else if (result == 3)
            {
                return Ok(new { status = HttpStatusCode.NotFound, result = result, message = "Jatah cuti sudah habis" });
            }
            else if (result == 4)
            {
                return Ok(new { status = HttpStatusCode.OK, result = result, message = "Pengajuan cuti telah dikirim" });
            }
            else if (result == 5)
            {
                return Ok(new { status = HttpStatusCode.UnprocessableEntity, result = result, message = "Gagal mengirim pengajuan" });
            }
            else 
            {
                return Ok(new { status = HttpStatusCode.OK, result = result, message = "Tanggal cutinya kok gitu" });
            }
        }

        [HttpPut]
        [Route("Approval")]
        public ActionResult LeaveApproval(LeaveVM leaveApproval)
        {
            var result = leaveRepository.LeaveApproval(leaveApproval);
            if (result == 1)
            {
                return Ok(new { status = HttpStatusCode.OK, result = result, message = "Cuti Disetujui" });

            }
            else if (result == 2)
            {
                return Ok(new { status = HttpStatusCode.NotFound, result = result, message = "Data cuti tidak ditemukan" });
            }
            else if (result == 3)
            {
                return Ok(new { status = HttpStatusCode.OK, result = result, message = "Cuti Ditolak" });
            }
            else
            {
                return Ok(new { status = HttpStatusCode.BadRequest, result = result, message = "Error" });
            }
        }

        [HttpPut]
        [Route("Quota")]
        public ActionResult leaveQuota(LeaveVM leaveQuota)
        {
            var result = leaveRepository.LeaveQuota(leaveQuota);
            if (result > 1)
            {
                return Ok(new { status = HttpStatusCode.OK, result = result, message = "Masa Cuti berhasil ditambahkan" });
            }
            else if (result < 1)
            {
                return Ok(new { status = HttpStatusCode.NotFound, result = result, message = "Masa Cuti gagal ditambahkan" });
            }
            else
            {
                return Ok(new { status = HttpStatusCode.BadRequest, result = result, message = "Error" });
            }
        }
    }
}
