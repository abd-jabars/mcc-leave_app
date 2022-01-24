using API.Context;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class LeaveRepository : GeneralRepository<MyContext, Leave, int>
    {
        public LeaveRepository(MyContext myContext) : base(myContext)
        {

        }
    }
}
