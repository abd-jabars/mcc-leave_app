using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModel
{
    public class LeaveVM
    {
        public string NIK { get; set; }
        public int LeaveId { get; set; }
        public int LeaveStatus { get; set; }
        public int LeaveQuota { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Attachment { get; set; }
        public string managerNote { get; set; }
        public int Quota { get; set; }
        public int totalLeave { get; set; }
    }
}
