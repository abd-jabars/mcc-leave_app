using API.Context;
using API.Models;
using API.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class EmployeeRepository : GeneralRepository<MyContext, Employee, string>
    {
        private readonly MyContext myContext;
        public EmployeeRepository(MyContext myContext) : base (myContext)
        {
            this.myContext = myContext;
        }

        public int Register(RegisterVM register)
        {
            int increment = myContext.Employees.ToList().Count;
            string formatedNik = "";
            if (increment == 0)
            {
                formatedNik = DateTime.Now.ToString("yyyy") + "0" + increment.ToString();
            }
            else
            {
                string increment2 = myContext.Employees.ToList().Max(e => e.NIK);
                int formula = Int32.Parse(increment2) + 1;
                formatedNik = formula.ToString();
            }

            int countAccountRole = myContext.AccountRoles.ToList().Count;
            int formatedId;
            if (countAccountRole == 0)
            {
                formatedId = countAccountRole;
            }
            else
            {
                int countAccountRole2 = myContext.AccountRoles.ToList().Max(ar => ar.Id);
                int formula = countAccountRole2 + 1;
                formatedId = formula;
            }

            if (CheckEmail(register) == 1 && CheckPhone(register) == 1)
            {
                // email & phone already used
                return 1;
            }
            else if (CheckEmail(register) == 1)
            {
                //email already used
                return 2;
            }
            else if (CheckPhone(register) == 1)
            {
                // phone already used
                return 3;
            }
            else
            {
                var employee = new Employee
                {
                    NIK = formatedNik,
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Gender = register.Gender,
                    BirthDate = register.BirthDate,
                    Email = register.Email,
                    Phone = register.Phone,
                    ManagerId = register.ManagerId,
                    DepartmentId = register.DepartmentId
                };
                myContext.Employees.Add(employee);
                myContext.SaveChanges();

                var account = new Account
                { 
                    NIK = employee.NIK,
                    Password = BCrypt.Net.BCrypt.HashPassword(register.Password),
                    PrevLeaveQuota = 0,
                    LeaveQuota = 0,
                    LeaveStatus = false
                };
                myContext.Accounts.Add(account);
                myContext.SaveChanges();

                // var countAccountRole = myContext.AccountRoles.ToList().Count;
                var accountRole = new AccountRole
                {
                    //Id = formatedId,
                    AccountId = account.NIK,
                    RoleId = 3
                };
                myContext.AccountRoles.Add(accountRole);
                myContext.SaveChanges();

                return 0;
            }
        }

        public IEnumerable<Object> GetRegisteredData()
        {
            var registeredData = from employees in myContext.Employees
                                 join accounts in myContext.Accounts
                                    on employees.NIK equals accounts.NIK
                                 select new
                                 {
                                     NIK = employees.NIK,
                                     FullName = employees.FirstName + " " + employees.LastName,
                                     Gender = employees.Gender,
                                     BirthDate = employees.BirthDate.ToString("dddd, dd MMMM yyyy"),
                                     Email = employees.Email,
                                     Phone = employees.Phone,
                                     RoleName = myContext.AccountRoles.Where(acr => acr.AccountId == employees.NIK).Select(acr => acr.Role.Name).ToList()
                                 };
            return registeredData;
        }

        public RegisterVM GetRegisteredData(string NIK)
        {
            var query = myContext.Employees.Where(e => e.NIK == NIK)
                                    .Include(e => e.Account)
                                        .FirstOrDefault();
            if (query == null)
                return null;

            var registeredData = new RegisterVM
            {
                NIK = query.NIK,
                FirstName = query.FirstName,
                LastName = query.LastName,
                Gender = query.Gender,
                BirthDate = query.BirthDate,
                Email = query.Email,
                Phone = query.Phone,
                DepartmentId = query.DepartmentId,
                ManagerId = query.ManagerId,
                RoleName = myContext.AccountRoles.Where(acr => acr.AccountId== query.NIK).Select(acr => acr.Role.Name).ToList()
            };

            return registeredData;
        }

        public int UpdateRegisteredData(RegisterVM register)
        {
            if (CheckPhoneMail(register) == 1 || CheckPhoneMail(register) == 7)
            {
                // Phone already used
                return 1;
            }
            else if (CheckPhoneMail(register) == 3 || CheckPhoneMail(register) == 5)
            {
                // Email already used
                return 2;
            }
            else if (CheckPhoneMail(register) == 4)
            {
                // Phone and email already used
                return 3;
            }
            else
            {
                var employee = new Employee
                {
                    NIK = register.NIK,
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Gender = register.Gender,
                    BirthDate = register.BirthDate,
                    Email = register.Email,
                    Phone = register.Phone,
                    ManagerId = register.ManagerId,
                    DepartmentId = register.DepartmentId
                };
                myContext.Entry(employee).State = EntityState.Modified;
                myContext.SaveChanges();

                return 0;
            }
        }

        public int CheckEmail(RegisterVM register)
        {
            var checkEmail = myContext.Employees.Where(e => e.Email == register.Email).FirstOrDefault();
            if (checkEmail != null)
                return 1;
            else
                return 0;
        }

        public int CheckPhone(RegisterVM register)
        {
            var checkPhone = myContext.Employees.Where(e => e.Phone == register.Phone).FirstOrDefault();
            if (checkPhone != null)
                return 1;
            else
                return 0;
        }

        public int CheckPhoneMail(RegisterVM register)
        {
            var checkData = myContext.Employees.Where(e => e.NIK == register.NIK).FirstOrDefault();
            if (checkData != null)
            {
                myContext.Entry(checkData).State = EntityState.Detached;
            }
            if (checkData.Email == register.Email)
            {
                if (checkData.Phone == register.Phone) // email & phone still the same => update
                {
                    return 0;
                }
                else
                {
                    if (CheckPhone(register) == 1) // same email, different phone but already used => can't update, phone already used
                    {
                        return 1;
                    }
                    else // same email, different phone but never used => update
                    {
                        return 2;
                    }
                }
            }
            else // change email
            {
                if (CheckEmail(register) == 1) // email already used
                {
                    if (checkData.Phone == register.Phone) // even though phone still the same => can't update, email already used
                    {
                        return 3;
                    }
                    else //either phone is different
                    {
                        if (CheckPhone(register) == 1) // phone already used => can't update, email & phone already used
                        {
                            return 4;
                        }
                        else // phone never used => can't update, email already used
                        {
                            return 5;
                        }
                    }
                }
                else // email never used
                {
                    if (checkData.Phone == register.Phone) // phone still the same => update
                    {
                        return 6;
                    }
                    else // change phone
                    {
                        if (CheckPhone(register) == 1) // phone already used => can't update, phone already used
                        {
                            return 7;
                        }
                        else // phone never used => update
                        {
                            return 8;
                        }
                    }
                }
            }
        }

    }
}
