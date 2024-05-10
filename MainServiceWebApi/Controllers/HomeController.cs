using HelpersDTO.AppointmentDto.Enums;
using HelpersDTO.Authentication;
using MainServiceWebApi.Helpers;
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
                    SinceDate = _dateTimeProvider.GetNow(),
                    Statuses = [(int)StatusEnum.Free]
                };
                var appointments = await _service.GetActiveAppointnmentsAsync(request);
                if(appointments?.Any() ?? false)
                    return View(appointments.Select(app => app.ToAppointmentVM()).ToList());
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка на главной странице {mainMessage}", e.Message);
            }
            return RedirectToAction("Index", "Error");
        }
    }
}
