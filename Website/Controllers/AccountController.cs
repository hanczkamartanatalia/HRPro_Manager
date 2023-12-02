using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Website.Entities;
using Website.Service;
using Website.Service.AccountService;

namespace Website.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index(int id)
        {
            if (id <= 0) return RedirectToAction("Login");
            LoginData loginData = EntityService<LoginData>.GetById(id);
            return View(loginData);
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
                int id = loginDb.Id;
                return RedirectToAction("Index", new { id = id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login");
            }

        }

    }
}
