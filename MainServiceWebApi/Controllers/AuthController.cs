using HelpersDTO.Authentication.DTO.Models;
using HelpersDTO.Patient.DTO;
using MainServiceWebApi.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using HelpersDTO.Patient;
using Services.Abstractions;

namespace MainServiceWebApi.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IApplicationConfig _config;
        private readonly ITokenService _tokenService;
        ILogger<AuthController> _logger;
        private readonly IRequestClient<CreatePatientRequest> _client;

        public AuthController(
            IAccountService accountService,
            IApplicationConfig config,
            ITokenService tokenService,
            ILogger<AuthController> logger,
            IRequestClient<CreatePatientRequest> client)
        {
            _accountService = accountService;
            _config = config;
            _tokenService = tokenService;
            _logger = logger;
            _client = client;
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
                    var loginResponse = await _accountService.LoginAsync(new LoginDTO()
                    {
                        Email = login.Email ?? "",
                        Password = login.Password ?? "",
                        PhoneNumber = ""
                    });

                    if (!loginResponse?.Flag ?? true)
                    {
                        ModelState.AddModelError("", loginResponse?.Message ?? "При авторизации произошла ошибка. Повторите попытку");
                        return View("Index", login);
                    }

                    var result = await _tokenService.Validate(loginResponse!.token);
                    if (result)
                    {
                        HttpContext.Response.Cookies.Append(_config.CookiesName, loginResponse!.token!);

                        if (!string.IsNullOrEmpty(login.ReturnUrl))
                            Redirect(login.ReturnUrl);
                        return RedirectToAction("Profile", "Patient");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "При авторизации произошла ошибка");
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
                    var registerResponse = await _accountService.RegisterAsync(new RegisterDTO()
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

                    if ((!registerResponse?.Flag ?? true) || (registerResponse!.Messages?.Any() ?? false))
                    {
                        if (registerResponse == null)
                        {
                            ModelState.AddModelError("", "Сервис недоступен. Попробуйте позднее");
                        }
                        registerResponse?.Messages?.ForEach(message => ModelState.AddModelError("", message));
                        return View(model);
                    }

                    // Добавление пациента в PatientService
                    var userId = registerResponse.UserId;
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

                        var request = new CreatePatientRequest() 
                        {
                            Patient = patientDto
                        };

                        var response = await _client.GetResponse<CreatePatientResponse>(request);

                        if (response == null)
                        {
                            _logger.LogError("Не удалось получить ответ от PatientService для CreatePatientRequest");
                            ModelState.AddModelError("", "При попытке создания пациента произошла ошибка. Попробуйте еще раз.");
                            return View(model);
                        }

                        _logger.LogInformation("Получен ответ CreatePatientRequest {response}", response.Message);
                        if (response?.Message?.Success == false)
                        {
                            ModelState.AddModelError("","При попытке создания пациента произошла ошибка");
                        }
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
