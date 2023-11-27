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
            LoginData loggedInUser = TempData["LoggedInUser"] as LoginData;
            return View(loggedInUser);
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult ProcessLogin(LoginData model)
        {
            try
            {
                LoginData loginDb = LoginService.Login(model.Login, model.Password);
                TempData["LoggedInUser"] = loginDb;
                HttpContext.Session.SetInt32("ID", loginDb.Id);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Login");
            }

        }

    }
}
