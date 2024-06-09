using HelpersDTO.AppointmentDto.Enums;
using HelpersDTO.Doctor.DTO.Models;
using HelpersDTO.Patient;
using HelpersDTO.Patient.DTO;
using MainServiceWebApi.Areas.Admin.Helpers;
using MainServiceWebApi.Areas.Admin.Models;
using MainServiceWebApi.Helpers;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace MainServiceWebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppointmentController(
        ILogger<AppointmentController> logger,
        IAppointmentService appointmentService,
        IDoctorService doctorService,
        IDateTimeProvider dateTimeProvider,
        IPatientService patientService,
        IRequestClient<CreateNewPassportRequest> client) : Controller
    {
        private readonly ILogger<AppointmentController> _logger = logger;
        private readonly IAppointmentService _appointmentService = appointmentService;
        private readonly IDoctorService _doctorService = doctorService;
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly IPatientService _patientService = patientService;
        private readonly IRequestClient<CreateNewPassportRequest> _client = client;

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
                    if (doctorVM != null && result != null)
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

        public async Task<IActionResult> UpdateStatusAsync(Guid id, int status)
        {
            try
            {
                var result = await _appointmentService.UpdateStatusAsync(id, status);
                if (result)
                    return RedirectToAction("Appointments", "Admin");
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка {message}", e.Message);
            }
            return View();
        }
    }
}
