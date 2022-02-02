using API.Context;
using API.Models;
using API.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class LeaveRepository : GeneralRepository<MyContext, Models.Leave, int>
    {
        private readonly MyContext myContext;
        public LeaveRepository(MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }

        public int LeaveRequest(LeaveVM leaveRequest)
        {
            var employee = myContext.Employees.Find(leaveRequest.NIK);
            if (employee == null)
                return 1; // employee data not found

            var leave = myContext.Leaves.Find(leaveRequest.LeaveId);
            if (leave == null)
                return 2; // leave data not found

            var account = myContext.Accounts.Where(a => a.NIK == employee.NIK).FirstOrDefault();

            if (leave.Type == leaveType.normal)
            {
                // periksa jatah cuti
                if ((account.LeaveQuota + account.PrevLeaveQuota) < 1)
                    return 3; // jatah cuti habis
                else if (((account.LeaveQuota + account.PrevLeaveQuota) - leaveRequest.totalLeave) < 1)
                {
                    return 3; // jatah cuti kurang
                }
                else if (leaveRequest.totalLeave < 0)
                {
                    return 6; // jatah cuti minus
                }

                // jatah cuti masih ada dan simpan data ke database
                if (SubmitForm(leaveRequest) == 1) // tanggal pengajuan logis
                {
                    // kirim email
                    if (SendEmailRequest(employee, leaveRequest) == 1)
                    {
                        return 4; // pengajuan berhasil dikirim
                    }
                    else
                    {
                        return 5; // gagal mengirim pengajuan
                    }
                }
                else
                {
                    return 6; // tanggal pengajuan cuti tidak logis
                }
            }
            else
            {
                if (leave.Period <= leaveRequest.totalLeave)
                {
                    return 3; // jatah cuti kurang
                }
                // cuti spesial langsung simpan data ke database
                if (SubmitForm(leaveRequest) == 1) // tanggal pengajuan logis
                {
                    // kirim email
                    if (SendEmailRequest(employee, leaveRequest) == 1)
                    {
                        return 4; // pengajuan berhasil dikirim
                    }
                    else
                    {
                        return 5; // gagal mengirim pengajuan
                    }
                }
                else
                {
                    return 6; // tanggal pengajuan cuti tidak logis
                }
            }
        }

        public int LeaveApproval(LeaveVM leaveApproval)
        {
            var acc = myContext.Accounts.AsNoTracking().Where(a => a.NIK == leaveApproval.NIK).FirstOrDefault();
            var temp = myContext.LeaveEmployees.AsNoTracking().Where(le => le.Id == leaveApproval.LeaveId).FirstOrDefault();
            var leave = myContext.Leaves.AsNoTracking().Where(l => l.Id == temp.LeaveId).FirstOrDefault();
            var totalLeave = acc.LeaveQuota;
            if (temp != null)
            {

                //myContext.Entry(myContext.LeaveEmployees).State = EntityState.Detached;

                if (leave.Type == leaveType.normal)
                //cuti normal
                {
                    totalLeave = (acc.LeaveQuota + acc.PrevLeaveQuota) - Convert.ToInt32((temp.EndDate - temp.StartDate).TotalDays);
                }

                if (leaveApproval.LeaveStatus == 1)
                {
                    acc.LeaveQuota = totalLeave;
                    temp.Status = Approval.Disetujui;
                    myContext.Entry(acc).State = EntityState.Modified;
                    myContext.Entry(temp).State = EntityState.Modified;
                    var result = myContext.SaveChanges();
                    return 1; //approval approved saved
                }
                else
                {
                    temp.Status = Approval.Ditolak;
                    myContext.Entry(temp).State = EntityState.Modified;
                    var result = myContext.SaveChanges();
                    return 3; //approval declined saved
                }
            }
            else
            {
                //Leave Approval not found
                return 2;
            }
        }

        public void LeaveQuotaTransfer()
        {
            Console.WriteLine("LeaveQuotaTransfer is Running...");
            var date = DateTime.Now;
            var year = date.Year;
            DateTime expDate = new DateTime(year, 07, 01);
            DateTime nyDate = new DateTime(year, 01, 01);
            var acc = myContext.Accounts;
            try
            {
                if (date >= expDate)
                {
                    Console.WriteLine("Previous Leave Quota is Expired...");
                    foreach (var i in acc)
                    {
                        i.PrevLeaveQuota = 0;
                        myContext.Entry(i).State = EntityState.Modified;
                    }
                }

                if (date == nyDate)
                {
                    Console.WriteLine("Leave Quota is moved to previous leave quota...");
                    foreach (var i in acc)
                    {
                        i.PrevLeaveQuota = i.LeaveQuota;
                        myContext.Entry(i).State = EntityState.Modified;
                    }
                }
                myContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int LeaveQuota(LeaveVM leaveQuota)
        {
            var acc = myContext.Accounts;
            if (acc != null)
            {
                foreach (var i in acc)
                {
                    i.LeaveQuota = leaveQuota.Quota;
                    myContext.Entry(i).State = EntityState.Modified;
                }

                var result = myContext.SaveChanges();
                return result; //New Quota saved succeed
            }
            else
            {
                return 0; //New Quota error
            }
        }

        public int SubmitForm(LeaveVM leaveRequest)
        {
            int countAccountRole = myContext.LeaveEmployees.ToList().Count;
            int formatedId;
            if (countAccountRole == 0)
            {
                formatedId = countAccountRole;
            }
            else
            {
                int countAccountRole2 = myContext.LeaveEmployees.ToList().Max(ar => ar.Id);
                int formula = countAccountRole2 + 1;
                formatedId = formula;
            }
            if (leaveRequest.StartDate < leaveRequest.EndDate)
            {
                var leaveEmployee = new LeaveEmployee
                {
                    //Id = formatedId,
                    StartDate = leaveRequest.StartDate,
                    EndDate = leaveRequest.EndDate,
                    Attachment = leaveRequest.Attachment,
                    NIK = leaveRequest.NIK,
                    LeaveId = leaveRequest.LeaveId,
                    Status = 0
                };
                myContext.LeaveEmployees.Add(leaveEmployee);
                myContext.SaveChanges();
                return 1;
            }
            return 0;
        }

        public int SendEmailRequest(Employee employee, LeaveVM leaveRequest)
        {
            var manager = myContext.Employees.Where(e => e.NIK == employee.ManagerId).FirstOrDefault();

            string from = "mccreg61net@gmail.com";
            string pwdFrom = "61mccregnet";
            string to = manager.Email;

            var leave = myContext.Leaves.Find(leaveRequest.LeaveId);

            // set body
            var department = myContext.Departments.Where(d => d.Id == employee.DepartmentId).FirstOrDefault();
            var emailBody = $"Yth. {manager.FirstName} {manager.LastName},\n" +
                            $"Manager Departemen {department.Name}\n" +
                            $"di Tempat\n\n" +
                            $"Yang bertanda tangan di bawah ini:\n\n" +
                            $"NIK \t: {employee.NIK}\n" +
                            $"Nama \t: {employee.FirstName} {employee.LastName}\n\n" +
                            $"dengan ini saya bermaksud untuk mengajukan {leave.Name}, " +
                            $"terhitung mulai tanggal {leaveRequest.StartDate.ToString("dd MMMM yyyy")} sampai dengan {leaveRequest.EndDate.ToString("dd MMMM yyyy")}.\n\n" +
                            $"Demikianlah surat pengajuan ini saya buat untuk dapat dipertimbangkan sebagaimana mestinya. Atas izin yang diberikan saya ucapkan terima kasih.\n\n" +
                            $"Hormat saya,\n\n" +
                            $"{employee.FirstName} {employee.LastName}\n" +
                            $"NIK.{employee.NIK}";

            // email message
            MailMessage mailMessage = new MailMessage(from, to);
            mailMessage.Subject = $"Pengajuan Cuti - {employee.FirstName} {employee.LastName}";
            //mailMessage.Body = $"Tipe Cuti: {leave.Type}. Body: {leaveRequest.Attachment}";
            mailMessage.Body = emailBody;

            // set smtp  
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(from, pwdFrom),
                EnableSsl = true
            };

            // send email
            try
            {
                client.Send(mailMessage);
                return 1;
            }
            catch (SmtpException)
            {
                return 2;
            }
        }

        public IEnumerable<object> GetNormalLeave()
        {
            var lList = myContext.Leaves;

            var query = from leave in lList
                        where leave.Type == leaveType.normal
                        select new
                        {
                            leave
                        };
            return query;
        }
        public IEnumerable<object> GetSpecialLeave()
        {
            var lList = myContext.Leaves;

            var query = from leave in lList
                        where leave.Type == leaveType.special
                        select new
                        {
                            leave
                        };
            return query;
        }
    }
}
