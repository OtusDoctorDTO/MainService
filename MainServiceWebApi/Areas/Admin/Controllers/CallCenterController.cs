using HelpersDTO.AppointmentDto.Enums;
using HelpersDTO.Authentication;
using HelpersDTO.Patient.DTO;
using MainServiceWebApi.Areas.Admin.Helpers;
using MainServiceWebApi.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using System;
using static MassTransit.ValidationResultExtensions;

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

        public async Task<IActionResult> AppointmentsAsync()
        {
            return await Task.Run(() => View(new AppointmentPanelViewModel()
            {
                Statuses = new Dictionary<StatusEnum, bool>
                {
                    { StatusEnum.Free, false },
                    { StatusEnum.BookedByUser, false },
                    { StatusEnum.Confirmed, false },
                    { StatusEnum.Waiting, false },
                    { StatusEnum.InProccess, false },
                    { StatusEnum.Success, false },
                    { StatusEnum.Сanceled, false }
                }
            }));
        }

        [HttpPost]
        public async Task<IActionResult> AppointmentsAsync(AppointmentPanelViewModel? search = null)
        {
            try
            {
                search ??= new AppointmentPanelViewModel();
                if (!search.Statuses?.Any() ?? false)
                {
                    search.Statuses = new Dictionary<StatusEnum, bool>
                    {
                        { StatusEnum.Free, false },
                        { StatusEnum.BookedByUser, false },
                        { StatusEnum.Confirmed, false },
                        { StatusEnum.Waiting, false },
                        { StatusEnum.InProccess, false },
                        { StatusEnum.Success, false },
                        { StatusEnum.Сanceled, false }
                    };
                }

                search.AppointmentsSearchResult = null;
                var request = new ShortAppointmentRequest()
                {
                    Count = search.Count,
                    SinceDate = search.StartDate ?? _dateTimeProvider.GetNow(),
                    ForDate = search.EndDate ?? _dateTimeProvider.GetNow().AddDays(1),
                    Statuses = search.Statuses?.Where(status => status.Value == true).Select(status => (int)status.Key).ToArray()
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

    }
}
