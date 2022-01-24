using API.Context;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class LeaveEmployeeRepository : GeneralRepository<MyContext, LeaveEmployee, int>
    {
        public LeaveEmployeeRepository(MyContext myContext) : base(myContext)
        {

        }
    }
}
