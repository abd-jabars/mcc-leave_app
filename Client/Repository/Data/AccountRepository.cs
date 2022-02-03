using API.Models;
using Client.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Repository.Data
{
    public class AccountRepository : GeneralRepository<Account, string>
    {
        private readonly Address address;
        private readonly string request;
        private readonly HttpClient httpClient;
        public AccountRepository(Address address, string request = "Accounts/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.link)
            };
        }
    }
}
