using API.Models;
using API.ViewModel;
using Client.Base;
using Client.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class LoginController : BaseController<Account, LoginRepository, string>
    {
        private readonly LoginRepository loginRepository;
        public LoginController(LoginRepository repository) : base(repository)
        {
            this.loginRepository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Auth(LoginVM login)
        {
            var jwtToken = await repository.Auth(login);
            var token = jwtToken.idToken;

            if (token == null)
            {
                return RedirectToAction("index", "Login");
            }

            HttpContext.Session.SetString("JWToken", token);

            return RedirectToAction("index", "SbAdmin");
        }

    }
}
