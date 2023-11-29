using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Website.Entities;
using Website.Service.AccountService;

namespace Website.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index(LoginData loginDb)
        {

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

        public IActionResult ProcessLogin(LoginData model)
        {
            try
            {
                LoginData loginDb = LoginService.Login(model.Login, model.Password);
                return RedirectToAction("Index", loginDb);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login");
            }

        }

    }
}
