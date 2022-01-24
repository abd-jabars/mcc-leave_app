using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("tb_tr_accountroles")]
    public class AccountRole
    {
        [Key]
        public int Id { get; set; }

        [JsonIgnore]
        public virtual Account Account { get; set; }
        public string AccountId { get; set; }

        [JsonIgnore]
        public virtual Role Role { get; set; }
        public int RoleId { get; set; }
    }
}
