using API.Context;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class DepartmentRepository : GeneralRepository<MyContext, Department, string>
    {
        public DepartmentRepository(MyContext myContext) : base(myContext)
        {

        }
    }
}
