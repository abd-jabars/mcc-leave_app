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
    public class AccountRepository : GeneralRepository<MyContext, Account, string>
    {
        private readonly MyContext myContext;
        public AccountRepository(MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }

        public int Login(Login login)
        {
            var getEmail = myContext.Employees.Where(emp => emp.Email == login.Email).FirstOrDefault();
            if (getEmail != null)
            {
                var account = myContext.Accounts.Where(acc => acc.NIK == getEmail.NIK).FirstOrDefault();
                var verifyPasswd = BCrypt.Net.BCrypt.Verify(login.Password, account.Password);
                if (verifyPasswd == true)
                    return 1; // login success
                else
                    return 2; // wrong password
            }
            else
            {
                return 0; // wrong email
            }
        }

        public IEnumerable<Object> GetRoles(Login login)
        {
            var getMail = myContext.Employees.Where(employee => employee.Email == login.Email).FirstOrDefault();
            var getRoleName = myContext.AccountRoles.Where(acr => acr.AccountId == getMail.NIK).Select(acr => acr.Role.Name).ToList();
            return getRoleName;
        }

        public int ForgotPassword(ForgotPassword forgotPassword)
        {
            var checkEmail = myContext.Employees.Where(emp => emp.Email == forgotPassword.Email).FirstOrDefault();
            if (checkEmail != null)
            {
                Random random = new Random();
                int otp = random.Next(100_000, 999_999);
                DateTime expiredToken = DateTime.Now.AddMinutes(2);
                bool isUsed = false;

                // set account attibute
                var account = myContext.Accounts.Where(acc => acc.NIK == checkEmail.NIK).FirstOrDefault();
                if (account != null)
                {
                    account.NIK = account.NIK;
                    account.Password = account.Password;
                    account.OTP = otp;
                    account.ExpiredToken = expiredToken;
                    account.IsUsed = isUsed;

                    myContext.Entry(account).State = EntityState.Modified;
                    myContext.SaveChanges();
                    if (SendEmail(checkEmail, account) == 1)
                    {
                        return 1; // successfully sent email
                    }
                    return 2; // failed to send email
                }
                return 3; // account not found
            }
            return 0; // email not found
        }

        public int SendEmail(Employee employee, Account account)
        {
            string from = "mccreg61net@gmail.com";
            string pwdFrom = "61mccregnet";
            string to = employee.Email;

            // email message
            MailMessage mailMessage = new MailMessage(from, to);
            mailMessage.Subject = "Forgot Password";
            mailMessage.Body = $"Hello {employee.FirstName} {employee.LastName}. \n\nThis is the otp to change your password: {account.OTP}. \nPlease use it before {string.Format("{0:hh:mm tt}", account.ExpiredToken)}"; // {account.ExpiredToken.Hour}.{account.ExpiredToken.Minute}";

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

        public int ChangePassword(ForgotPassword forgotPassword)
        {
            // email received -> insert otp + set datetime when user insert the otp
            var checkEmail = myContext.Employees.Where(emp => emp.Email == forgotPassword.Email).FirstOrDefault();
            if (checkEmail != null)
            {
                var account = myContext.Accounts.Where(acc => acc.NIK == checkEmail.NIK).FirstOrDefault();
                if (account != null)
                {
                    // check otp expired date
                    if (account.ExpiredToken >= DateTime.Now)
                    {
                        // check otp
                        if (account.OTP == forgotPassword.OTP)
                        {
                            // update password
                            if (account.IsUsed == false)
                            {
                                account.Password = BCrypt.Net.BCrypt.HashPassword(forgotPassword.Password);
                                account.IsUsed = true;
                                myContext.Entry(account).State = EntityState.Modified;
                                myContext.SaveChanges();
                                return 1; // otp true n never used
                            }
                            return 2; // otp true but already used
                        }
                        return 3; // wrong otp
                    }
                    return 4; // otp expired
                }
                //return 0;
            }
            return 0;
        }

    }
}
