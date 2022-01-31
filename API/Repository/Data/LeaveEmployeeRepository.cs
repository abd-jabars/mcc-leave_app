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
        private readonly MyContext myContext;
        public LeaveEmployeeRepository(MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }
        public IEnumerable<object> GetLeaveEmployee(int id)
        {
            var empList = myContext.Employees;
            var accList = myContext.Accounts;
            var deptList = myContext.Departments;
            var lList = myContext.Leaves;
            var leList = myContext.LeaveEmployees;

            var query = from emp in empList
                        join acc in accList
                        on emp.NIK equals acc.NIK
                        join le in leList
                        on emp.NIK equals le.NIK
                        join l in lList
                        on le.LeaveId equals l.Id
                        join dept in deptList
                        on emp.DepartmentId equals dept.Id
                        where le.Id == id
                        select new
                        {
                            nik = emp.NIK,
                            fullName = emp.FirstName + " " + emp.LastName,
                            emp.Phone,
                            emp.Email,
                            dept.Id,
                            totalLeave = Convert.ToInt32((le.EndDate - le.StartDate).TotalDays),
                            endDate = le.EndDate.ToString("dd/MM/yyyy"),
                            startDate = le.StartDate.ToString("dd/MM/yyyy"),
                            l.Type,
                            le.Attachment,
                            le.Status
                        };
            return query;
        }
        public IEnumerable<object> GetApprovalList()
        {
            var empList = myContext.Employees;
            var accList = myContext.Accounts;
            var lList = myContext.Leaves;
            var deptList = myContext.Departments;
            var leList = myContext.LeaveEmployees;

            var query = from emp in empList
                        join acc in accList
                        on emp.NIK equals acc.NIK
                        join le in leList
                        on emp.NIK equals le.NIK
                        join l in lList
                        on le.LeaveId equals l.Id
                        join dept in deptList
                        on emp.DepartmentId equals dept.Id
                        where le.Status == 0
                        select new
                        {
                            nik = emp.NIK,
                            fullName = emp.FirstName + " " + emp.LastName,
                            deptId = dept.Id,
                            totalLeave = Convert.ToInt32((le.EndDate - le.StartDate).TotalDays),
                            endDate = le.EndDate.ToString("dd/MM/yyyy"),
                            startDate = le.StartDate.ToString("dd/MM/yyyy"),
                            l.Type,
                            le.Id,
                            le.Attachment,
                            le.Status
                        };
            return query;
        }
        public IEnumerable<object> GetHistoryList()
        {
            var empList = myContext.Employees;
            var accList = myContext.Accounts;
            var lList = myContext.Leaves;
            var deptList = myContext.Departments;
            var leList = myContext.LeaveEmployees;

            var query = from emp in empList
                        join le in leList
                        on emp.NIK equals le.NIK
                        join l in lList
                        on le.LeaveId equals l.Id
                        join dept in deptList
                        on emp.DepartmentId equals dept.Id
                        where le.Status == 0
                        select new
                        {
                            nik = emp.NIK,
                            fullName = emp.FirstName + " " + emp.LastName,
                            deptId = dept.Id,
                            totalLeave = Convert.ToInt32((le.EndDate - le.StartDate).TotalDays),
                            endDate = le.EndDate.ToString("dd/MM/yyyy"),
                            startDate = le.StartDate.ToString("dd/MM/yyyy"),
                            l.Type,
                            le.Id,
                            le.Attachment,
                            le.Status
                        };
            return query;
        }

        public IEnumerable<object> GetByManager(String nik)
        {
            var empList = myContext.Employees;
            var leList = myContext.LeaveEmployees;

            var query = from emp in empList
                        join le in leList
                        on emp.NIK equals le.NIK
                        where emp.ManagerId == nik
                        select new
                        {
                            leList
                        };
            return query;
        }
    }
}
