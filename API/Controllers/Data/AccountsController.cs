using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController<Account, AccountRepository, string>
    {
        private readonly AccountRepository accountRepository;
        public IConfiguration _configuration;
        public AccountsController(AccountRepository accountRepository, IConfiguration configuration) : base(accountRepository)
        {
            this.accountRepository = accountRepository;
            this._configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult Login(LoginVM login)
        {
            // var code = 0;
            string message;
            var result = accountRepository.Login(login);
            switch (result)
            {
                case 1:
                    {
                        var getRole = accountRepository.GetRoles(login);
                        var getNik = accountRepository.GetNik(login);
                        var getName = accountRepository.GetName(login);

                        var claims = new List<Claim>
                {
                    new Claim("Email", login.Email)
                };
                        foreach (var item in getRole)
                        {
                            claims.Add(new Claim("roles", item.ToString()));
                        };
                        claims.Add(new Claim("nik", getNik));
                        claims.Add(new Claim("name", getName));

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var siginIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(10),
                            signingCredentials: siginIn
                            );
                        var idToken = new JwtSecurityTokenHandler().WriteToken(token);

                        claims.Add(new Claim("TokenSecurity", idToken.ToString()));
                        return Ok(new JWTokenVM { status = HttpStatusCode.OK, token = idToken, message = "Login succes" });
                    }
                case 2:
                    {
                        message = $"Wrong Password";
                        return Ok(new JWTokenVM { status = HttpStatusCode.Forbidden, token = null, message = message });
                    }
                case 3:
                    {
                        message = $"Email not found";
                        return Ok(new JWTokenVM { status = HttpStatusCode.NotFound, token = null, message = message });
                    }
                default:
                    {
                        message = $"Login Failed";
                        return Ok(new JWTokenVM { status = HttpStatusCode.BadRequest, token = null, message = message });
                    }
            }
        }

        [HttpPut]
        [Route("ForgotPassword")]
        public ActionResult ForgotPassword(ForgotPasswordVM forgotPassword)
        {
            var result = accountRepository.ForgotPassword(forgotPassword);
            if (result == 1)
            {
                return Ok(new { status = HttpStatusCode.OK, result = result, message = "OTP code has been sent to your email. Please check your email." });
            }
            else if (result == 2)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, result = result, message = "Failed to sent otp code to your email" });
            }
            else
            {
                return NotFound(new { status = HttpStatusCode.NotFound, result = result, message = "Email not registered, may be you type a wrong email or you can register first." });
            }
        }

        [HttpPut]
        [Route("ChangePassword")]
        public ActionResult ChangePassword(ForgotPasswordVM forgotPassword)
        {
            var result = accountRepository.ChangePassword(forgotPassword);
            if (result == 1)
            {
                return Ok(new { status = HttpStatusCode.OK, result = result, message = "Password changed. Now you can login with the new password.", caseNumber = 1 });
            }
            else if (result == 2)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, result = result, message = "Otp already used. Please re-request the otp to do this action again.", caseNumber = 3 });
            }
            else if (result == 3)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, result = result, message = "Wrong otp. Please correct the otp before expired.", caseNumber = 2 });
            }
            else if (result == 4)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, result = result, message = "Otp expired. Please re-request the otp.", caseNumber = 3 });
            }
            return BadRequest(new { status = HttpStatusCode.BadRequest, result = result, message = "Wrong email. Please correct your email before otp expired.", caseNumber = 2 });
        }

    }
}
