using MainServiceWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MainServiceWebApi.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ILogger<AppointmentController> _logger;
        public AppointmentController(ILogger<AppointmentController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(DoctorViewModel doctor)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {

                }
                return RedirectToAction("Index", "Auth", new{ returnUrl = "Appointment/Index"});
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }
    }
}
