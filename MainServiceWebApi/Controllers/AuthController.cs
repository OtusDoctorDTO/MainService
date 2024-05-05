using MainServiceWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MainServiceWebApi.Controllers
{
    public class AuthController : Controller
    {
        public AuthController()
        {
        }
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckInAsync(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {

            }
            return RedirectToAction("Index", "Home");
        }
    }
}
