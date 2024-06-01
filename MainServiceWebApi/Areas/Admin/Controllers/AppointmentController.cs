using HelpersDTO.Doctor.DTO.Models;
using MainServiceWebApi.Areas.Admin.Models;
using MainServiceWebApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using MainServiceWebApi.Areas.Admin.Helpers;

namespace MainServiceWebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppointmentController : Controller
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorService _doctorService;
        private readonly IDateTimeProvider _dateTimeProvider;
        public AppointmentController(ILogger<AppointmentController> logger, IAppointmentService appointmentService, IDoctorService doctorService, IDateTimeProvider dateTimeProvider)
        {
            _logger = logger;
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<IActionResult> InfoAsync(Guid id)
        {
            try
            {
                var appointment = await _appointmentService.GetById(id);
                AppointmentFullInfoViewModel? result = null;
                FullInfoDoctorDTO? doctor = null;
                if (appointment != null)
                {
                    doctor = await _doctorService.GetById(appointment!.DoctorId);
                    var doctorVM = doctor?.ToFullDoctorInfoVM(_dateTimeProvider.GetNow());
                    //var patient = ;
                    result = appointment.ToAppointmentFullInfoVM();
                    if(doctorVM != null && result != null)
                        result!.Doctor = doctorVM!;
                    return View(result);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка {message}", e.Message);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MakeAppointmentAsync(AppointmentFullInfoViewModel appointment)
        {
            try
            {

            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка {message}", e.Message);
            }
            return View(appointment);
        }
    }
}
