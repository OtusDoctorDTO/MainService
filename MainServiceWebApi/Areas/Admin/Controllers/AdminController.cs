using HelpersDTO.Authentication;
using HelpersDTO.Authentication.DTO.Models;
using HelpersDTO.Base.Models;
using HelpersDTO.CallCenter.DTO.Models;
using HelpersDTO.Doctor.DTO.Models;
using HelpersDTO.Patient;
using HelpersDTO.Patient.DTO;
using MainServiceWebApi.Areas.Admin.Helpers;
using MainServiceWebApi.Areas.Admin.Models;
using MainServiceWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using HelpersDTO.Doctor;
using HelpersDTO.Doctor.DTO;
using MassTransit;

namespace MainServiceWebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly IMainService _mainService;
        private readonly IPatientService _patientService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<AdminController> _logger;
        private readonly IDoctorService _doctorService;
        private readonly IAccountService _accountService;
        private readonly IRequestClient<CreateDoctorRequest> _client;

        public AdminController(IMainService service, 
            IDateTimeProvider dateTimeProvider, 
            ILogger<AdminController> logger, 
            IPatientService patientService, 
            IDoctorService doctorService, 
            IAccountService accountService, 
            IRequestClient<CreateDoctorRequest> client)
        {
            _mainService = service;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
            _patientService = patientService;
            _doctorService = doctorService;
            _accountService = accountService;
            _client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AppointmentsAsync()
        {
            return await Task.Run(() => View(new AppointmentPanelViewModel()));
        }

        [HttpPost]
        public async Task<IActionResult> AppointmentsAsync(AppointmentPanelViewModel? search = null)
        {
            try
            {
                search ??= new AppointmentPanelViewModel();
                search.AppointmentsSearchResult = null;
                var request = new ShortAppointmentRequest()
                {
                    Count = search.Count,
                    SinceDate = search.StartDate ?? _dateTimeProvider.GetNow(),
                    ForDate = search.EndDate ?? _dateTimeProvider.GetNow().AddDays(1),
                    Statuses = search.Status != null ? new int[] { search.Status!.Value } : null
                };

                // ***для теста
                request.SinceDate = DateTime.Now.AddYears(-1);
                request.ForDate = DateTime.Now.AddYears(1);
                //***

                var appointments = await _mainService.GetActiveAppointnmentsAsync(request);
                if (appointments?.Any() ?? false)
                {
                    var userIds = appointments!
                        .Where(app => app.PatientId != null)
                        .Select(app => app.PatientId!.Value).Distinct()
                        .ToArray();

                    List<PatientDTO>? patients = null;
                    if (userIds?.Any() ?? false)
                    {
                        patients = await _patientService.GetByIdsAsync(userIds!);
                    }
                    search.AppointmentsSearchResult = appointments!
                        .Select(app => app
                        .ToAppointmentViewModel(patients?.FirstOrDefault(p => p.Id == app.PatientId))).ToList();
                    return View(search);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка при получении списка записей для подтверждения {Message}", e.Message);
            }
            return View(search);
        }

        [HttpGet]
        public IActionResult AddDoctor()
        {
            return View(new DoctorRegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDoctor(DoctorRegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Регистрация пользователя
                    var registerResponse = await _accountService.RegisterAsync(new RegisterDTO()
                    {
                        Email = model.Email ?? "",
                        Password = model.Password ?? "",
                        Phone = model.Phone,
                        Role = new RoleDTO()
                        {
                            Name = Domain.Entities.Constants.Doctor
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

                    // Добавление врача в DoctorService
                    var userId = registerResponse.UserId;
                    if (userId != null)
                    {
                        //var existingDoctor = await _doctorService.GetDoctorByUserIdAsync(userId.Value);
                        //if (existingDoctor != null)
                        //{
                        //    ModelState.AddModelError("", "Врач с таким пользователем уже существует.");
                        //    return View(model);
                        //}

                        var doctor = new NewDoctorDTO
                        {
                            Phone = model.Phone,
                            Email = model.Email,
                            UserId = userId!.Value,
                            User = new BaseUserDTO
                            {
                                FirstName = model.FirstName,
                                LastName = model.LastName
                            },
                            Specialty = model.Specialty
                        };

                        var request = new CreateDoctorRequest()
                        {
                            Doctor = doctor
                        };

                        var response = await _client.GetResponse<CreateDoctorResponse>(request);

                        if (response != null)
                        {
                            _logger.LogInformation("Получен ответ CreateDoctorResponse {response}", response.Message);
                            if (response?.Message?.Success == false)
                            {
                                ModelState.AddModelError("", "При попытке создания пациента произошла ошибка");
                            }
                        }
                        else
                        {
                            _logger.LogError("Не удалось получить ответ от DoctorService для CreateDoctorResponse");
                            ModelState.AddModelError("", "При попытке создания пациента произошла ошибка. Попробуйте еще раз.");
                            return View(model);
                        }

                        //await _doctorService.AddDoctorAsync(doctor);
                    }

                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("При попытке добавить врача произошла ошибка {error}", e.Message);
                ModelState.AddModelError("", "Произошла ошибка при добавлении врача. Попробуйте еще раз.");
            }
            return View(model);
        }

    }
}
