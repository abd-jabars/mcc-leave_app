using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModel
{
    public class ForgotPassword
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int OTP { get; set; }
    }
}
