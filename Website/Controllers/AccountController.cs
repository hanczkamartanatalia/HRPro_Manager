using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult ProcessLogin()
        {
            return RedirectToAction("Index");
        }

    }
}
