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

        [HttpPost]
        public IActionResult Index(DoctorViewModel doctor)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {

                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }
    }
}
