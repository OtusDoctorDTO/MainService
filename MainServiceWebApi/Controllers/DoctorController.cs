using MainServiceWebApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace MainServiceWebApi.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<DoctorController> _logger;

        public DoctorController(IDoctorService doctorService, IDateTimeProvider dateTimeProvider, ILogger<DoctorController> logger)
        {
            _doctorService = doctorService;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
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
    }
}
