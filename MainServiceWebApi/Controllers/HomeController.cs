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
                // получение свободных записей с сегодняшнего дня на неделю вперед
                var request = new ShortAppointmentRequest()
                {
                    Count = 20,
                    // AddYears(-1), надо удалить, когда можно админ сможет добавлять рассписание врачей и создавать записи
                    SinceDate = _dateTimeProvider.GetNow().AddYears(-1),
                    ForDate = _dateTimeProvider.GetNow().AddDays(7).AddYears(1),
                    Statuses = [(int)StatusEnum.Free]
                };
                var appointments = await _service.GetActiveAppointnmentsAsync(request);
                if (appointments?.Any() ?? false)
                {
                    var doctorsIds = appointments!.Select(app => app.DoctorId).Distinct().ToArray();
                    var doctors = await _service.GetDoctorsByIds(doctorsIds);
                    if (doctors?.Any() ?? false)
                    {
                        var dataVM = doctors?.Select(doctor => new MainPageViewModel()
                        {
                            DoctorId = doctor.Id,
                            FullName = $"{doctor.User.LastName} {doctor.User.FirstName} {doctor.User.MiddleName}".Trim(),
                            Appointments = appointments
                            .Where(app => app.DoctorId == doctor.Id)
                            .GroupBy(app => app.Date)
                            .Select(x => new ShortAppointmentViewModel()
                            {
                                Date = x.Key,
                                Data = x.OrderBy(x => x.Time).ToDictionary(x => x.Id, y => y.Time)
                            }).ToList()
                        }).ToList();
                        return View(dataVM);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка на главной странице {mainMessage}", e.Message);
            }
            _logger.LogWarning("Список врачей пуст");
            return RedirectToAction("Index", "Error");
        }
    }
}
