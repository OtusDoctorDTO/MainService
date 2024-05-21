using HelpersDTO.AppointmentDto.Enums;
using HelpersDTO.Authentication;
using MainServiceWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace MainServiceWebApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMainService _service;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ILogger<HomeController> logger,
            IMainService service,
            IDateTimeProvider dateTimeProvider
            )
        {
            _logger = logger;
            _service = service;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var request = new ShortAppointmentRequest()
                {
                    Count = 20,
                    ForDate = _dateTimeProvider.GetNow().AddDays(5),
                    // удалить тестовые данные .AddDays(-60)
                    SinceDate = _dateTimeProvider.GetNow().AddDays(-60),
                    Statuses = [(int)StatusEnum.Free]
                };
                var appointments = await _service.GetActiveAppointnmentsAsync(request);
                if (appointments?.Any() ?? false)
                {
                    var doctorsIds = appointments!.Select(app => app.DoctorId).Distinct().ToArray();
                    var doctors = await _service.GetDoctorsByIds(doctorsIds);
                    var dataVM = doctors.Select(doctor => new MainPageViewModel()
                    {
                        DoctorId = doctor.Id,
                        FullName = $"{doctor.User.LastName} {doctor.User.FirstName} {doctor.User.MiddleName}".Trim(),
                        Appointments = appointments
                        .GroupBy(app => app.DoctorId).Select(x => new ShortAppointmentViewModel()
                        {
                            Date = x.First().StartDate,
                            Data = x.ToDictionary(x => x.Id, y => y.StartDate.TimeOfDay)
                        }).ToList()
                    }).ToList();

                    return View(dataVM);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка на главной странице {mainMessage}", e.Message);
            }
            return RedirectToAction("Index", "Error");
        }
    }
}
