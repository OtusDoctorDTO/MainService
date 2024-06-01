using HelpersDTO.AppointmentDto.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace MainServiceWebApi.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(ILogger<AppointmentController> logger, IAppointmentService appointmentService)
        {
            _logger = logger;
            _appointmentService = appointmentService;
        }

        public async Task<IActionResult> MakeAppointment(Guid id)
        {
            try
            {
                if (User?.Identity?.IsAuthenticated ?? false)
                {
                    var principal = HttpContext.User;
                    var nameIdentifier = HttpContext.User.Claims.FirstOrDefault(c => c.Type.EndsWith("claims/nameidentifier"))?.Value;
                    if (string.IsNullOrEmpty(nameIdentifier) || !Guid.TryParse(nameIdentifier, out Guid userId))
                        throw new NullReferenceException($"Ошибка получения данных пользователя: {HttpContext.User.Identity}");
                    var result = await _appointmentService.UpdateStatusAsync(id, userId, (int)StatusEnum.Success);
                    if (result)
                        return View("Sucsess");
                }
                else
                    return RedirectToAction("Index", "Auth", new { returnUrl = $"Appointment/MakeAppointment?id={id}" });
            }
            catch (Exception e)
            {
                _logger.LogError("При попытке записи произошла ошибка {message}", e.Message);
            }
            return RedirectToAction("Index", "Error");
        }
    }
}
