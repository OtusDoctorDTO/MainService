using HelpersDTO.Doctor.DTO.Models;
using MainServiceWebApi.Areas.Admin.Models;
using MainServiceWebApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using MainServiceWebApi.Areas.Admin.Helpers;
using HelpersDTO.AppointmentDto.Enums;
using System.Threading.Tasks;

namespace MainServiceWebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppointmentController : Controller
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorService _doctorService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IPatientService _patientService;
        public AppointmentController(ILogger<AppointmentController> logger, IAppointmentService appointmentService, IDoctorService doctorService, IDateTimeProvider dateTimeProvider, IPatientService patientService)
        {
            _logger = logger;
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _dateTimeProvider = dateTimeProvider;
            _patientService = patientService;
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
                    result = appointment.ToAppointmentFullInfoVM();
                    var doctorTask = _doctorService.GetById(appointment!.DoctorId);
                    var patientTask = _patientService.GetPatientProfileAsync(appointment.PatientId!.Value);
                    doctor = await doctorTask;
                    var patient = await patientTask;
                    var doctorVM = doctor?.ToFullDoctorInfoVM(_dateTimeProvider.GetNow());  
                    if(doctorVM != null && result != null)
                        result!.Doctor = doctorVM!;
                    if (patient != null)
                        result!.Patient = patient.ToPatientVM();
                    return View(result);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка {message}", e.Message);
            }
            return View();
        }

        public async Task<IActionResult> ConfirmAsync(Guid id, bool isConfirm)
        {
            try
            {
                var status = isConfirm ? StatusEnum.Waiting : StatusEnum.Сanceled;
                var result = await _appointmentService.UpdateStatusAsync(id, (int)status);
                if (result)
                    return RedirectToAction("NewAppointments", "CallCenter");
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка {message}", e.Message);
            }
            return View();
        }
    }
}
