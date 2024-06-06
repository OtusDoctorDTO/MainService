using HelpersDTO.Authentication.DTO.Models;
using HelpersDTO.Patient.DTO;
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
        private readonly IPatientService _patientService;
        ILogger<AuthController> _logger;

        public AuthController(
            IAccountService accountService,
            IApplicationConfig config,
            ITokenService tokenService,
            IPatientService patientService, 
            ILogger<AuthController> logger)
        {
            _accountService = accountService;
            _config = config;
            _tokenService = tokenService;
            _patientService = patientService;
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
                        if (!string.IsNullOrEmpty(login.ReturnUrl))
                            Redirect(login.ReturnUrl);
                        return RedirectToAction("Profile", "Patient");
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
            try
            {
                if (ModelState.IsValid)
                {
                    // захешировать пароль
                    var registerResponce = await _accountService.RegisterAsync(new RegisterDTO()
                    {
                        Email = model.Email ?? "",
                        Password = model.Password ?? "",
                        Phone = model.Phone,
                        RememberMe = model.RememberMe,
                        Role = new RoleDTO()
                        {
                            Name = Domain.Entities.Constants.User
                        }
                    });

                    if ((!registerResponce?.Flag ?? true) || (registerResponce!.Messages?.Any() ?? false))
                    {
                        if (registerResponce == null)
                        {
                            ModelState.AddModelError("", "Сервис недоступен. Попробуйте позднее");
                        }
                        registerResponce?.Messages?.ForEach(message => ModelState.AddModelError("", message));
                        return View(model);
                    }


                    // Добавление пациента в PatientService
                    //TODO переделать на асинхронное взаимодействие
                    var userId = registerResponce.UserId;
                    if (userId != null)
                    {
                        var patientDto = new PatientDTO
                        {
                            UserId = userId!.Value,
                            FirstName = null,
                            LastName = null,
                            Phone = model.Phone,
                            Email = model.Email,
                            IsNew = true
                        };
                        await _patientService.AddPatientAsync(patientDto);
                    }

                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                        Redirect(model.ReturnUrl);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("При попытке совершить регистрацию произошла ошибка {register}", e.Message);
                ModelState.AddModelError("", "Произошла неизвестная ошибка. Попробуйте еще раз");
            }
            return View(model);
        }
    }
}
