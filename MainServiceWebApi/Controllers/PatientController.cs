using HelpersDTO.Authentication.DTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MainServiceWebApi.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //private readonly IPatientServiceClient _patientServiceClient;

        //public PatientController(IPatientServiceClient patientServiceClient)
        //{
        //    _patientServiceClient = patientServiceClient;
        //}


        //[HttpGet]
        //[Authorize]
        //public async Task<IActionResult> Profile()
        //{
        //    var token = HttpContext.Session.GetString("Token");
        //    var user = await _patientServiceClient.GetProfileAsync(token);
        //    return View(user);
        //}

        //public IActionResult Logout()
        //{
        //    HttpContext.Session.Remove("Token");
        //    return RedirectToAction("Login");
        //}
    }
}
