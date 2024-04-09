using MainServiceWebApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace MainServiceWebApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMainService _service;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IMainService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var doctors = await _service.GetDoctors();
            return View(doctors.Select(doc => doc.ToDoctorVM()).ToList());
        }
    }
}
