using HelpersDTO.AppointmentDto;
using HelpersDTO.AppointmentDto.DTO;
using HelpersDTO.Patient;
using MainServiceWebApi.Areas.Admin.Models;
using MainServiceWebApi.Models;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Services.Implementations;

namespace MainServiceWebApi.Controllers
{
    [Authorize]
    public class PatientController(IPatientService patientServiceClient, ILogger<PatientController> logger, IDoctorService doctorService, IAppointmentService appointmentService, IRequestClient<CreateAppointmentDtoRequest> client) : Controller
    {
        private readonly IPatientService _patientService = patientServiceClient;
        private readonly IDoctorService _doctorService = doctorService;
        private readonly IAppointmentFactory _appointmentFactory;
        private readonly ILogger<PatientController> _logger = logger;
        private readonly IAppointmentService _appointmentService = appointmentService;
        private readonly IRequestClient<CreateAppointmentDtoRequest> _client = client;

        public async Task<IActionResult> Profile()
        {
            try
            {
                var principal = HttpContext.User;
                var nameIdentifier = HttpContext.User.Claims.First(c => c.Type.EndsWith("claims/nameidentifier")).Value;
                var userId = Guid.Parse(nameIdentifier);
                var patient = await _patientService.GetPatientProfileAsync(userId);
                if (patient == null)
                    return RedirectToAction("Index", "Error");

                var doctors = await _doctorService.GetAllDoctorsAsync();
                var appointments = await _appointmentService.GetAppointmentsByPatientIdAsync(userId);
                var selectedDoctorAppointments = await _appointmentService.GetAppointmentsByDoctorIdAsync(patient.SelectedDoctorId);


                var model = new PatientViewModel
                {
                    FirstName = patient!.FirstName ?? "",
                    LastName = patient!.LastName ?? "",
                    Email = patient.Email ?? "",
                    UserId = userId,
                    PhoneNumber = patient.Phone ?? "",
                    Doctors = doctors.Select(d => new DoctorViewModel { Id = d.Id, LastName = d.User.LastName }).ToList(),
                    Appointments = appointments.Select(a => new AppointmentViewModel
                    {
                        Id = a.Id,
                        DoctorName = a.DoctorId.ToString(),
                        Date = a.Date,
                        Time = a.Time,
                        Status = a.Status.ToString()
                    }).ToList(),
                    SelectedDoctorId = patient.SelectedDoctorId,
                    SelectedDoctorAppointments = selectedDoctorAppointments
                };
                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Произошла ошибка");
                ModelState.AddModelError("", "Произошла неизвестная ошибка. Попробуйте еще раз");
            }
            return RedirectToAction("Index", "Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookAppointment(PatientViewModel model)
        {
            try
            {
                var appointmentDto = new AppointmentDto
                {
                    PatientId = model.UserId,
                    DoctorId = model.DoctorId,
                    Date = DateOnly.FromDateTime(model.AppointmentDate),
                    Time = TimeOnly.FromDateTime(model.AppointmentTime)
                };

                var request = new CreateAppointmentDtoRequest()
                {
                    Appointment = new CreatingAppointmentDto { DoctorId = model.DoctorId , PatientId = GetCurrentPatientId(), Time = model.AppointmentTime }
                };

                var response = await _client.GetResponse<CreateAppointmentDtoResponse>(request);

                if (response != null)
                {
                    _logger.LogInformation("Получен ответ CreateAppointmentDtoResponse {response}", response.Message);
                    if (response?.Message?.Success == false)
                    {
                        ModelState.AddModelError("", "При попытке создания произошла ошибка");
                    }
                }
                else
                {
                    _logger.LogError("Не удалось получить ответ");
                    ModelState.AddModelError("", "При попытке создания произошла ошибка. Попробуйте еще раз.");
                }



                //var success = await _appointmentService.BookAppointmentAsync(appointmentDto);
                //model.BookingSuccess = success;

                // Обновление списка записей к выбранному врачу
                model.SelectedDoctorAppointments = await _appointmentService.GetAppointmentsByDoctorIdAsync(model.DoctorId);

                return RedirectToAction("Profile");
            }
            catch (Exception e)
            {
                _logger.LogError("Ошибка при попытке записи к врачу: {error}", e.Message);
                ModelState.AddModelError("", "Произошла ошибка при попытке записи к врачу. Попробуйте еще раз.");
                return View("Profile", model);
            }
        }
        private Guid GetCurrentPatientId()
        {
            var nameIdentifierClaim = HttpContext.User.Claims
            .FirstOrDefault(c => c.Type.EndsWith("nameidentifier"));

            if (nameIdentifierClaim == null)
            {
                throw new Exception("Не удалось найти идентификатор пользователя в claims.");
            }

            if (!Guid.TryParse(nameIdentifierClaim.Value, out var userId))
            {
                throw new Exception("Идентификатор пользователя имеет неверный формат.");
            }

            return userId;
        }
    }
}
