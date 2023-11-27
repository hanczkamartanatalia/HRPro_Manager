using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Website.Entities;
using Website.Service.AccountService;

namespace Website.Controllers
{
    public class AccountController : Controller
    {
        LoginData loginDb;

        public IActionResult Index()
        {
            // gubi mój obiekt
            // tutaj loginDb = null
            return View(loginDb);
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult ProcessLogin(LoginData model)
        {
            try
            {
                loginDb = LoginService.Login(model.Login, model.Password);
                HttpContext.Session.SetInt32("ID", loginDb.Id);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return RedirectToAction("Login");
            }

        }

    }
}
