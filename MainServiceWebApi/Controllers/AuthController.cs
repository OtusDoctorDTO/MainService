using Domain.Entities;
using HelpersDTO.Authentication.DTO.Models;
using MainServiceWebApi.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Providers.Contracts;
using Services.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MainServiceWebApi.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IApplicationConfig _config;
        public AuthController(IAccountService accountService, IApplicationConfig config)
        {
            _accountService = accountService;
            _config = config;
        }
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckInAsync(LoginViewModel login)
        {
            try
            {



                if (ModelState.IsValid)
                {
                    var loginResponce = await _accountService.LoginAsync(new LoginDTO()
                    {
                        Email = login.Email ?? "",
                        Password = login.Password ?? "",
                        PhoneNumber = ""
                    });

                    if (!loginResponce?.Flag ?? true)
                    {
                        ModelState.AddModelError("", loginResponce?.Message ?? "При авторизации произошла ошибка. Повторите попытку");
                        return View(login);
                    }

                    var tokenHandler = new JwtSecurityTokenHandler();

                    var validationOptions = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = _config.AuthOptions.Issuer,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.AuthOptions.Key)),
                        NameClaimType = "aud",
                        ClockSkew = TimeSpan.FromSeconds(30)
                    };
                    var tokenValidationResult = await tokenHandler.ValidateTokenAsync(loginResponce!.token, validationOptions);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var registerResponce = await _accountService.RegisterAsync(new RegisterDTO()
                {
                    Email = model.Email,
                    Password = model.Password,
                    Phone = model.Phone,
                    Role = new RoleDTO()
                    {
                        Name = Domain.Entities.Constants.User
                    }
                });

                if (!registerResponce?.Flag ?? false)
                {
                    registerResponce?.Messages.ForEach(message => ModelState.AddModelError("", message));
                    return View(model);
                }
            }




            return RedirectToAction("Index", "Home");
        }
    }
}
