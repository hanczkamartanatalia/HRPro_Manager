using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Website.Database;
using Website.Entities;
using Website.Models;
using Website.Service;
using Website.Service.AccountService;

namespace Website.Controllers
{
    public class AccountController : Controller
    {
        private AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int? id = HttpContext.Session.GetInt32("LD_Id");
            if (id <= 0 || id == null) return RedirectToAction("Login");
            LoginData loginData = EntityService<LoginData>.GetById((int)id);

            return View();
        }
        public IActionResult Login()
        {
            if(LoginService.isLogin(HttpContext.Session.GetInt32("LD_Id"))) return RedirectToAction("Index");
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
                User user = EntityService<User>.GetById(loginDb.Id_User);
                Role role = EntityService<Role>.GetById(loginDb.Id_Role);

                HttpContext.Session.Clear();

                HttpContext.Session.SetInt32("U_Id", user.Id);
                HttpContext.Session.SetInt32("LD_Id", loginDb.Id);
                HttpContext.Session.SetString("LD_Login", loginDb.Login);
                HttpContext.Session.SetString("U_Name", user.Name);
                HttpContext.Session.SetString("U_LastName", user.LastName);
                HttpContext.Session.SetString("U_Email", user.Email);
                HttpContext.Session.SetString("R_Name", role.Name);


                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                HttpContext.Session.SetString("Error","Incorrect login or password.");
                return RedirectToAction("Login");
            }
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SavePassword(LoginData loginData)
        {
            try
            {
                int? login_ID = HttpContext.Session.GetInt32("LD_Id");
                LoginData editLoginData = _context.LoginData.FirstOrDefault(i => i.Id == login_ID);

                editLoginData.Password = PasswordService.HashPassword(loginData.Password);

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ErrorViewModel errorModel = new ErrorViewModel { ErrorMessage = $"Error: {ex.Message}" };
                return View("Error", errorModel);
            }

        }
    }
}
