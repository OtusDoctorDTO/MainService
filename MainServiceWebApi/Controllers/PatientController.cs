using Microsoft.AspNetCore.Mvc;

namespace MainServiceWebApi.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
