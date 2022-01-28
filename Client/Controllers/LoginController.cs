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
            var token = HttpContext.Session.GetString("JWToken");

            if (token == null)
            {
                return View();
            }

            return RedirectToAction("DataTable", "Employees");
        }

        public IActionResult ChangePassword()
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

            Console.WriteLine(code);

            if (token == null)
            {
                TempData["code"] = code;
                TempData["msg"] = message;
                return RedirectToAction("index");
            }

            TempData["code"] = null;
            HttpContext.Session.SetString("JWToken", token);
            //HttpContext.Session.SetString("Name", jwtHandler.GetName(token));
            //HttpContext.Session.SetString("ProfilePicture", "assets/img/theme/user.png");

            return RedirectToAction("index", "home");
        }

        [HttpPut]
        public JsonResult ForgotPassword(ForgotPasswordVM forgotPassword)
        {
            var result = repository.ForgotPassword(forgotPassword);
            return Json(result);
        }

        [HttpPut]
        public JsonResult ChangePassword(ForgotPasswordVM forgotPassword)
        {
            var result = repository.ChangePassword(forgotPassword);
            return Json(result);
        }

    }
}
