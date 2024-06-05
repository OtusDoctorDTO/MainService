using MainServiceWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace MainServiceWebApi.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly ILogger<PatientController> _logger;

        public PatientController(IPatientService patientServiceClient, ILogger<PatientController> logger)
        {
            _patientService = patientServiceClient;
            _logger = logger;
        }

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
                var model = new PatientViewModel
                {
                    FirstName = patient!.FirstName ?? "",
                    LastName = patient!.LastName ?? "",
                    Email = patient!.Email ?? "",
                    UserId = userId
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
    }
}
