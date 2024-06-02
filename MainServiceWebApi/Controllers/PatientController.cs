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
        private readonly IPatientService _patientServiceClient;

        public PatientController(IPatientService patientServiceClient)
        {
            _patientServiceClient = patientServiceClient;
        }

        [HttpGet("Profile/{userId}")]
        public async Task<IActionResult> Profile(Guid userId)
        {
            var patient = await _patientServiceClient.GetPatientProfileAsync(userId);
            if (patient == null)
                return NotFound();

            var model = new PatientViewModel
            {
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Email = patient.Email,
                UsersId = userId
            };

            return View(model);
        }
    }

}
