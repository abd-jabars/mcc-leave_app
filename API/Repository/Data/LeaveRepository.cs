using API.Context;
using API.Models;
using API.ViewModel;
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

            if (leave.Type == "Cuti Normal")
            {
                // periksa jatah cuti
                if (account.LeaveQuota < 1)
                    return 3; // jatah cuti habis

                // jatah cuti masih ada dan simpan data ke database
                SubmitForm(leaveRequest);
                // kirim email
                SendEmailRequest(employee, leaveRequest);
                return 4;
            }
            else
            {
                // cuti spesial langsung simpan data ke database
                SubmitForm(leaveRequest);
                // kirim email
                SendEmailRequest(employee, leaveRequest);
                return 5;
            }
        }

        public void SubmitForm(LeaveVM leaveRequest)
        {
            var leaveEmployee = new LeaveEmployee
            {
                Id = myContext.LeaveEmployees.ToList().Count,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                Attachment = leaveRequest.Attachment,
                NIK = leaveRequest.NIK,
                LeaveId = leaveRequest.LeaveId,
                Status = 0
            };
            myContext.LeaveEmployees.Add(leaveEmployee);
            myContext.SaveChanges();
        }

        public int SendEmailRequest(Employee employee, LeaveVM leaveRequest)
        {
            var manager = myContext.Employees.Where(e => e.NIK == employee.ManagerId).FirstOrDefault();

            string from = "mccreg61net@gmail.com";
            string pwdFrom = "61mccregnet";
            string to = manager.Email;

            var leave = myContext.Leaves.Find(leaveRequest.LeaveId);

            // email message
            MailMessage mailMessage = new MailMessage(from, to);
            mailMessage.Subject = "Pengajuan Cuti";
            mailMessage.Body = $"Tipe Cuti: {leave.Type}. Body: {leaveRequest.Attachment}";

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

    }
}
