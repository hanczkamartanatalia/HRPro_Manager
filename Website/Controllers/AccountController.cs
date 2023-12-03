using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Website.Entities;
using Website.Service;
using Website.Service.AccountService;

namespace Website.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            int? id = HttpContext.Session.GetInt32("UserId");
            if (id <= 0 || id == null) return RedirectToAction("Login");
            LoginData loginData = EntityService<LoginData>.GetById((int)id);
            return View(loginData);
        }
        public IActionResult Login()
        {
            if(LoginService.isLogin(HttpContext.Session.GetInt32("UserId"))) return RedirectToAction("Index");
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ProcessLogin(LoginData model)
        {
            try
            {
                LoginData loginDb = LoginService.Login(model.Login, model.Password);
                int id = loginDb.Id;
                HttpContext.Session.SetInt32("UserId", id);
                Role role = EntityService<Role>.GetById(loginDb.Id_Role);
                HttpContext.Session.SetString("Role", role.Name);
                //return RedirectToAction("Index", new { id = id });
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Login");
            }

        }

    }
}
