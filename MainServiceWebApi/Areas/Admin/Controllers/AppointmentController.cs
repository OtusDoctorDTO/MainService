using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace MainServiceWebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppointmentController : Controller
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(ILogger<AppointmentController> logger, IAppointmentService appointmentService)
        {
            _logger = logger;
            _appointmentService = appointmentService;
        }

        public async Task<IActionResult> InfoAsync(Guid id)
        {
            try
            {
                var appointment = await _appointmentService.GetById(id);
                //var patient = ;
                //var doctor = ;
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка {message}", e.Message);
            }
            return RedirectToAction("Index", "Error");
        }

        public async Task<IActionResult> MakeAppointmentAsync(Guid id)
        {
            try
            {

            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка {message}", e.Message);
            }
            return RedirectToAction("Index", "Error");
        }
    }
}
