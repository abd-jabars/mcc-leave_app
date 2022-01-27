using API.Models;
using API.ViewModel;
using Client.Base;
using Client.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Client.Controllers
{
    [AllowAnonymous]
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

        [HttpPost("Login/Auth/")]
        public async Task<IActionResult> Auth(LoginVM login)
        {
            var jwtToken = await loginRepository.Auth(login);
            var token = jwtToken.token;
            var code = jwtToken.code;
            var message = jwtToken.message;

            Console.WriteLine(code);

            if (code == HttpStatusCode.NotFound)
            {
                TempData["code"] = code;
                TempData["msg"] = message;
            }
            else if (code == HttpStatusCode.Forbidden)
            {
                TempData["code"] = code;
                TempData["msg"] = message;
            }

            if (token == null)
            {
                return RedirectToAction("index");
            }

            TempData["code"] = null;
            HttpContext.Session.SetString("JWToken", token);
            //HttpContext.Session.SetString("Name", jwtHandler.GetName(token));
            //HttpContext.Session.SetString("ProfilePicture", "assets/img/theme/user.png");

            return RedirectToAction("index", "home");
        }
    }
}
