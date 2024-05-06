using MainServiceWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using HelpersDTO.Authentication.DTO.Models;

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
                var user = await _accountService.LoginAsync(new LoginDTO()
                {
                    Email = login.Email ?? "",
                    Password = login.Password ?? "",
                    PhoneNumber = ""
                });
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
