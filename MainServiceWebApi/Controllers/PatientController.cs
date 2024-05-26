using HelpersDTO.Authentication.DTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MainServiceWebApi.Models;
using Services.Abstractions;

namespace MainServiceWebApi.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet("Profile/{userId}")]
        public async Task<IActionResult> Profile(Guid userId)
        {
            try
            {
                var patient = await _patientService.GetPatientProfileAsync(userId);
                if (patient == null)
                    return NotFound();

                return View(patient);
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
