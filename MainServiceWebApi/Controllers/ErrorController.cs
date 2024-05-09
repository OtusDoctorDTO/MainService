using Microsoft.AspNetCore.Mvc;

namespace MainServiceWebApi.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
