using HelpersDTO.AppointmentDto.Enums;
using HelpersDTO.Authentication;
using MainServiceWebApi.Areas.Admin.Helpers;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using System;

namespace MainServiceWebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CallCenterController : Controller
    {
        private readonly IMainService _mainService;
        private readonly IPatientService _patientService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<CallCenterController> _logger;

        public CallCenterController(IMainService service, IDateTimeProvider dateTimeProvider, ILogger<CallCenterController> logger, IPatientService patientService)
        {
            _mainService = service;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
            _patientService = patientService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AppointmentsAsync(int? count = 20, DateTime? since = null, DateTime? forDate = null, int[]? statuses = null)
        {
            try
            {
                var request = new ShortAppointmentRequest()
                {
                    Count = count,
                    SinceDate = since ?? _dateTimeProvider.GetNow(),
                    ForDate = forDate ?? _dateTimeProvider.GetNow().AddDays(1),
                    Statuses = statuses
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
                        .Select(app => app.PatientId!.Value)
                        .ToArray();

                    if(userIds?.Any() ?? false)
                    {
                        var patients = await _patientService.GetByIdsAsync(userIds!);
                        var result = appointments?
                            .Select(app => app
                            .ToAppointmentViewModel(patients?
                                .FirstOrDefault(p=> p.Id == app.PatientId)))
                            .ToList();
                        return View(result);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка при получении списка записей для подтверждения {Message}", e.Message);
            }
            return View();
        }

    }
}
