using Domain.Entities;
using HelpersDTO.Authentication.DTO.Models;
using MainServiceWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace MainServiceWebApi.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAccountService _accountService;
        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckInAsync(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var loginResponce = await _accountService.LoginAsync(new LoginDTO()
                {
                    Email = login.Email ?? "",
                    Password = login.Password ?? "",
                    PhoneNumber = ""
                });

                if(!loginResponce?.Flag ?? true)
                {
                    ModelState.AddModelError("", loginResponce?.Message ?? "При авторизации произошла ошибка. Повторите попытку");
                    return View(login);
                }
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
                        Name = Constants.User
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
