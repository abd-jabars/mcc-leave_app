using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("tb_tr_leaveemployees")]
    public class LeaveEmployee
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Attachment { get; set; }
        public string managerNote { get; set; }
        public Approval Status { get; set; }

        //[JsonIgnore]
        [ForeignKey("NIK")]
        public virtual Employee Employee { get; set; }
        public string NIK { get; set; }
        
        //[JsonIgnore]
        public virtual Leave Leave { get; set; }
        public int LeaveId { get; set; }
    }

    public enum Approval
    { 
        Diproses,
        Disetujui,
        Ditolak
    }
}
