using Microsoft.AspNetCore.Mvc;

namespace MainServiceWebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CallCenterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
