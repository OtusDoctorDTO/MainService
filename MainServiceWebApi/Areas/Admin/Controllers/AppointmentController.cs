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
    public class AppointmentController : Controller
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorService _doctorService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IPatientService _patientService;
        IRequestClient<CreateNewPassportRequest> _client;
        public AppointmentController(
            ILogger<AppointmentController> logger,
            IAppointmentService appointmentService,
            IDoctorService doctorService,
            IDateTimeProvider dateTimeProvider,
            IPatientService patientService,
            IRequestClient<CreateNewPassportRequest> client)
        {
            _logger = logger;
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _dateTimeProvider = dateTimeProvider;
            _patientService = patientService;
            _client = client;
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
                    return RedirectToAction("Appointments", "CallCenter");
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка {message}", e.Message);
            }
            return View();
        }

        public async Task<IActionResult> CreateNewContractAsync(Guid id, Guid userId)
        {
            return await Task.Run(() => View(new CheckPatientViewModel(userId, id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewContractAsync(CheckPatientViewModel model)
        {
            if (ModelState.IsValid && model.Passport != null)
            {
                try
                {
                    var responce = await _client.GetResponse<CreateNewPassportDtoResponse>(new CreateNewPassportRequest()
                    {
                        Guid = Guid.NewGuid(),
                        Passport = new PassportDTO()
                        {
                            IssueDate = model.Passport?.IssueDate,
                            Number = model.Passport?.Number,
                            IssuedBy = model.Passport?.IssuedBy,
                            Series = model.Passport?.Series,
                            SubdivisionCode = model.Passport?.SubdivisionCode
                        },
                        UserId = model.UserId
                    });

                    _logger.LogInformation("Получен ответ CreateNewPassportDtoResponse {responce}", responce.Message);

                    if (responce.Message.Success && string.IsNullOrEmpty(responce.Message.Message))
                    {
                        return RedirectToAction("UpdateStatus", new { id = model.AppointmentId, status = (int)StatusEnum.Waiting });
                    }
                    else
                    {
                        ModelState.AddModelError("", responce.Message.Message ?? "При попытке сохранить данные произошла ошибка. Попробуйте позднее");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Произошла ошибка CreateNewPassportRequest");
                }
            }
            return View(model);
        }
    }
}
