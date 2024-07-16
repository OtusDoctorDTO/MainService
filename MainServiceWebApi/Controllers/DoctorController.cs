using HelpersDTO.AppointmentDto;
using HelpersDTO.AppointmentDto.DTO;
using HelpersDTO.Doctor.DTO.Models;
using MainServiceWebApi.Helpers;
using MainServiceWebApi.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using System.Numerics;

namespace MainServiceWebApi.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<DoctorController> _logger;
        private readonly IRequestClient<CreateAppointmentDtoRequest> _appointmentClient;

        public DoctorController(IDoctorService doctorService, IDateTimeProvider dateTimeProvider, ILogger<DoctorController> logger, IRequestClient<CreateAppointmentDtoRequest> appointmentClient)
        {
            _doctorService = doctorService;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
            _appointmentClient = appointmentClient;
        }

        public async Task<IActionResult> Info(Guid id)
        {
            try
            {
                var doctor = await _doctorService.GetById(id);
                if (doctor != null)
                {
                    var doctorVM = doctor.ToFullDoctorInfoVM(_dateTimeProvider.GetNow().AddDays(-7));
                    return View(doctorVM);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("При получении доктора {id} произошла ошибка {e}", id, e);
            }
            return RedirectToAction("Index", "Error");
        }

        [HttpGet]
        public IActionResult StartAppointment(Guid patientId)
        {
            var model = new StartAppointmentViewModel
            {
                PatientId = patientId
            };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartAppointment(StartAppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var request = new CreateAppointmentDtoRequest
                {
                    Appointment = new CreatingAppointmentDto
                    {
                        DoctorId = model.DoctorId,
                        PatientId = model.PatientId,
                        Complaints = model.Complaints,
                        Recommendations = model.Recommendations,
                        Time = DateTime.Now,
                        IsCompleted = false
                    }
                };

                try
                {
                    var response = await _appointmentClient.GetResponse<CreateAppointmentDtoResponse>(request);

                    if (response.Message.Success == true)
                    {
                        _logger.LogInformation("Прием начат успешно для пациента с ID {PatientId}", model.PatientId);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ошибка при начале приема");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при отправке запроса на начало приема");
                    ModelState.AddModelError("", "Произошла ошибка при обработке запроса");
                }
            }
            return View(model);
        }

        // GET: Doctor/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doctor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewDoctorDTO doctor)
        {
            if (ModelState.IsValid)
            {
                await _doctorService.AddDoctorAsync(doctor);
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }


    }
}
