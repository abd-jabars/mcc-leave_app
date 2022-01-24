using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("tb_m_employees")]
    public class Employee
    {
        [Key]
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        [JsonIgnore]
        public virtual ICollection<Employee> Employees { get; set; }

        [JsonIgnore]
        public virtual Employee Manager { get; set; }
        public string ManagerId { get; set; }

        [JsonIgnore]
        public virtual Department Department { get; set; }
        public string DepartmentId { get; set; }

        [JsonIgnore]
        public virtual ICollection<Department> Departments { get; set; }

        [JsonIgnore]
        public virtual ICollection<LeaveEmployee> LeaveEmployees { get; set; }
        public virtual Account Account { get; set; }
    }

    public enum Gender
    { 
        Pria,
        Wanita
    }
}
