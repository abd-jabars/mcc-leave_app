using API.Models;
using API.ViewModel;
using Client.Base;
using Client.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
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

        [Authorize]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("index", "Login");
        }

        [HttpPost("Login/Auth/")]
        public async Task<IActionResult> Auth(LoginVM login)
        {
            var jwtToken = await loginRepository.Auth(login);
            var token = jwtToken.token;
            var code = jwtToken.status;
            var message = jwtToken.message;

            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(token);

            var nik = decodedValue.Claims.First(c => c.Type == "nik").Value;
            var name = decodedValue.Claims.First(c => c.Type == "name").Value;

            if (token == null)
            {
                TempData["code"] = code;
                TempData["msg"] = message;
                return RedirectToAction("index");
            }

            TempData["code"] = null;
            TempData["nik"] = nik;
            TempData["name"] = name;
            HttpContext.Session.SetString("JWToken", token);
            HttpContext.Session.SetString("nik", nik);
            HttpContext.Session.SetString("name", name);
            //HttpContext.Session.SetString("ProfilePicture", "assets/img/theme/user.png");
            return RedirectToAction("index", "home");
        }
    }
}
