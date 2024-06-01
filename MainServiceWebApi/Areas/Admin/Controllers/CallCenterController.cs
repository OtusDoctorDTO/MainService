using HelpersDTO.AppointmentDto.Enums;
using HelpersDTO.Authentication;
using MainServiceWebApi.Areas.Admin.Helpers;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace MainServiceWebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CallCenterController : Controller
    {
        private readonly IMainService _service;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<CallCenterController> _logger;

        public CallCenterController(IMainService service, IDateTimeProvider dateTimeProvider, ILogger<CallCenterController> logger)
        {
            _service = service;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Appointments()
        {
            try
            {
                var request = new ShortAppointmentRequest()
                {
                    Count = 20,
                    SinceDate = _dateTimeProvider.GetNow(),
                    ForDate = _dateTimeProvider.GetNow().AddDays(7),
                    Statuses = [(int)StatusEnum.BookedByUser]
                };

                // удалить после теста
                request.SinceDate = _dateTimeProvider.GetNow().AddYears(-1);
                request.ForDate = _dateTimeProvider.GetNow().AddDays(7).AddYears(1);

                var appointments = await _service.GetActiveAppointnmentsAsync(request);
                return View(appointments?.Select(app => app.ToAppointmentViewModel()).ToList());
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка при получении списка записей для подтверждения {Message}", e.Message);
            }
            return View();
        }
    }
}
