using API.Models;
using API.ViewModel;
using Client.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Repository.Data
{
    public class EmployeeRepository : GeneralRepository<Employee, string>
    {
        private readonly Address address;
        private readonly string request;
        private readonly HttpClient httpClient;
        public EmployeeRepository(Address address, string request = "Employees/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.link)
            };
        }

        public Object Register(RegisterVM register)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(register), Encoding.UTF8, "application/json");

            Object entity = new Object();
            using (var response = httpClient.PostAsync(address.link + request + "Register", content).Result)
            {
                string apiResponse = response.Content.ReadAsStringAsync().Result;
                entity = JsonConvert.DeserializeObject<Object>(apiResponse);
            }
            return entity;
        }

        public async Task<Object> RegisteredData()
        {
            //List<RegisterVM> entities = new List<RegisterVM>();
            Object entities = new Object();

            using (var response = await httpClient.GetAsync(request + "RegisteredData"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entities = JsonConvert.DeserializeObject<Object>(apiResponse);
            }
            return entities;
        }

        public async Task<RegisterVM> RegisteredData(string NIK)
        {
            RegisterVM register = null;

            using (var response = await httpClient.GetAsync(request + "RegisteredData/" + NIK))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                register = JsonConvert.DeserializeObject<RegisterVM>(apiResponse);
            }
            return register;
        }

        public Object UpdateRegisteredData(RegisterVM register)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(register), Encoding.UTF8, "application/json");

            Object entity = new Object();
            using (var response = httpClient.PutAsync(request + "UpdateRegistered", content).Result)
            {
                string apiResponse = response.Content.ReadAsStringAsync().Result;
                entity = JsonConvert.DeserializeObject<Object>(apiResponse);
            }
            return entity;
        }

    }
}
