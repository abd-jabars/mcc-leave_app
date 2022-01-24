using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("tb_m_leaves")]
    public class Leave
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Period { get; set; }

        [JsonIgnore]
        public virtual ICollection<LeaveEmployee> LeaveEmployees { get; set; }
    }
}
