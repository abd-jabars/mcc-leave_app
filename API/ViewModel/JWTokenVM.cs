using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.ViewModel
{
    public class JWTokenVM
    {
        public HttpStatusCode status { get; set; }

        public string token { get; set; }

        public string message { get; set; }
        public string NIK { get; set; }
    }
}
