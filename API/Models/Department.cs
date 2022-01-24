using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("tb_m_departments")]
    public class Department
    {
        public string Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Employee> Employees { get; set; }

        [JsonIgnore]
        public virtual Employee Manager { get; set; }
        public string ManagerId { get; set; }
    }
}
