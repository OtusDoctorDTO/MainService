using HelpersDTO.Authentication;
using HelpersDTO.Authentication.DTO.Models;
using MainServiceWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace MainServiceWebApi.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IApplicationConfig _config;
        private readonly ITokenService _tokenService;
        ILogger<AuthController> _logger;


        public AuthController(
            IAccountService accountService,
            IApplicationConfig config,
            ITokenService tokenService,
            ILogger<AuthController> logger)
        {
            _accountService = accountService;
            _config = config;
            _tokenService = tokenService;
            _logger = logger;
        }
        public IActionResult Index(string? returnUrl = null)
        {
            return View(new LoginViewModel() { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckInAsync(LoginViewModel login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // захешировать пароль
                    var loginResponce = await _accountService.LoginAsync(new LoginDTO()
                    {
                        Email = login.Email ?? "",
                        Password = login.Password ?? "",
                        PhoneNumber = ""
                    });

                    if (!loginResponce?.Flag ?? true)
                    {
                        ModelState.AddModelError("", loginResponce?.Message ?? "При авторизации произошла ошибка. Повторите попытку");
                        return View("Index", login);
                    }

                    var result = await _tokenService.Validate(loginResponce!.token);
                    if (result)
                    {
                        HttpContext.Response.Cookies.Append(_config.CookiesName, loginResponce!.token!);
                        if(!string.IsNullOrEmpty(login.ReturnUrl))
                            Redirect(login.ReturnUrl);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "При авторазиции произошла ошибка");
            }
            ModelState.AddModelError("", "При авторизации произошла ошибка. Повторите попытку");
            return View("Index", login);
        }

        [HttpGet]
        public IActionResult Register(string? returnUrl = null)
        {
            return View(new RegisterViewModel() { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // захешировать пароль
                var registerResponce = await _accountService.RegisterAsync(new RegisterDTO()
                {
                    Email = model.Email,
                    Password = model.Password,
                    Phone = model.Phone,
                    RememberMe = model.RememberMe,
                    Role = new RoleDTO()
                    {
                        Name = Domain.Entities.Constants.User
                    }
                });

                if ((!registerResponce?.Flag ?? true) || registerResponce!.Messages.Any())
                {
                    registerResponce?.Messages.ForEach(message => ModelState.AddModelError("", message));
                    return View(model);
                }

                if (model.RememberMe)
                {
                    var result = await _tokenService.Validate(registerResponce!.Token);
                    if (result)
                        HttpContext.Response.Cookies.Append(_config.CookiesName, registerResponce!.Token!);
                }
            }
            if (!string.IsNullOrEmpty(model.ReturnUrl))
                Redirect(model.ReturnUrl);

            return RedirectToAction("Index", "Home");
        }
    }
}
