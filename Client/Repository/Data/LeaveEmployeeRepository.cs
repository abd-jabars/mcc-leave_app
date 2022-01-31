using API.Models;
using Client.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Repository.Data
{
    public class LeaveEmployeeRepository : GeneralRepository<LeaveEmployee, int>
    {
        private readonly Address address;
        private readonly string request;
        private readonly HttpClient httpClient;
        public LeaveEmployeeRepository(Address address, string request = "LeaveEmployees/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.link)
            };
        }

        public async Task<Object> GetByManager(string nik)
        {
            //List<RegisterVM> entities = new List<RegisterVM>();
            Object entities = new Object();

            using (var response = await httpClient.GetAsync(request + "GetByManager"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entities = JsonConvert.DeserializeObject<Object>(apiResponse);
            }
            return entities;
        }
    }
}
