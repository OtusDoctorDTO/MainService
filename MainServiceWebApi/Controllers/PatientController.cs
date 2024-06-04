using MainServiceWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;


namespace MainServiceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpGet("Profile/{userId}")]
        public async Task<IActionResult> Profile(Guid userId)
        {
            var model = new PatientViewModel { };
            try
            {
                if (ModelState.IsValid)
                {
                    var patient = await _patientService.GetPatientProfileAsync(userId);
                    if (patient == null)
                        return NotFound();

                    model = new PatientViewModel
                    {
                        FirstName = patient!.FirstName ?? "",
                        LastName = patient!.LastName ?? "",
                        Email = patient!.Email ?? "",
                        UserId = userId
                    };
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Произошла ошибка");
                ModelState.AddModelError("", "Произошла неизвестная ошибка. Попробуйте еще раз");
            }

            return View(model);
        }
    }
}
