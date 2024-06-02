using HelpersDTO.Patient.DTO;
using HelpersDTO.Patient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MainServiceWebApi.Models;
using Services.Abstractions;


namespace MainServiceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientServiceClient)
        {
            _patientService = patientServiceClient;
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
                        FirstName = patient.FirstName,
                        LastName = patient.LastName,
                        Email = patient.Email,
                        UserId = userId
                    };
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Произошла неизвестная ошибка. Попробуйте еще раз");
            }

            return View(model);
        }
    }
}
