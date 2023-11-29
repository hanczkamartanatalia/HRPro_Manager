using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Website.Entities;
using Website.Service.AccountService;

namespace Website.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            LoginData loginDb = TempData["LoginData"] as LoginData ?? new LoginData();

            if (loginDb == null)
            {
                return RedirectToAction("Login");
            }
            return View(loginDb);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcessLogin(LoginData model)
        {
            try
            {
                LoginData loginDb = LoginService.Login(model.Login, model.Password);
                TempData["LoginData"] = loginDb;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login");
            }

        }

    }
}
