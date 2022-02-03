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
    public class LeaveRepository : GeneralRepository<Leave, string>
    {
        private readonly Address address;
        private readonly string request;
        private readonly HttpClient httpClient;
        public LeaveRepository(Address address, string request = "Leaves/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.link)
            };
        }

        public async Task<Object> GetNormalLeave()
        {
            Object entities = new Object();

            using (var response = await httpClient.GetAsync(request + "normal"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entities = JsonConvert.DeserializeObject<Object>(apiResponse);
            }
            return entities;
        }

        public async Task<Object> GetSpecialLeave()
        {
            Object entities = new Object();

            using (var response = await httpClient.GetAsync(request + "special"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entities = JsonConvert.DeserializeObject<Object>(apiResponse);
            }
            return entities;
        }

        public Object LeaveRequest(LeaveVM leaveRequest)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(leaveRequest), Encoding.UTF8, "application/json");

            Object entity = new Object();
            using (var response = httpClient.PostAsync(address.link + request + "Request", content).Result)
            {
                string apiResponse = response.Content.ReadAsStringAsync().Result;
                entity = JsonConvert.DeserializeObject<Object>(apiResponse);
            }
            return entity;
        }

        public Object LeaveApproval(LeaveVM leaveApproval)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(leaveApproval), Encoding.UTF8, "application/json");

            Object entity = new Object();
            using (var response = httpClient.PutAsync(request + "Approval", content).Result)
            {
                string apiResponse = response.Content.ReadAsStringAsync().Result;
                entity = JsonConvert.DeserializeObject<Object>(apiResponse);
            }
            return entity;
        }

        public Object LeaveQuota(LeaveVM leaveQuota)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(leaveQuota), Encoding.UTF8, "application/json");

            Object entity = new Object();
            using (var response = httpClient.PutAsync(request + "Quota", content).Result)
            {
                string apiResponse = response.Content.ReadAsStringAsync().Result;
                entity = JsonConvert.DeserializeObject<Object>(apiResponse);
            }
            return entity;
        }

    }
}
