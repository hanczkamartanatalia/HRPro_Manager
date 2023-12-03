using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Data;
using Website.Database;
using Website.Entities;
using Website.Models.Account;
using Website.Service.AccountService;

namespace Website.Controllers
{
    public class UserController : Controller
    {
        private AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<User> users = _context.Users.ToList();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Register register)
        {
            User user = new User
            {
                Name = register.User.Name,
                LastName = register.User.LastName,
                Email = register.User.Email
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            Role role = (Role)_context.Roles.Where(i => i.Id == 3);

            if (role == null)
            {
                Console.WriteLine("Rola o Id=3 nie istnieje w bazie danych.");
                return RedirectToAction("Create");
            }

            //if (ModelState.IsValid)
            //{

            LoginData loginData = new LoginData
            (
                register.LoginData.Login,
                register.LoginData.Password,
                user.Id,
                user,
                role.Id,
                role
            );

            _context.LoginData.Add(loginData);
            _context.SaveChanges();
            _context.Dispose();
            return RedirectToAction("Index");
            //}

            //return View("Create");
        }

        public IActionResult Edit(int Id)
        {
            User editUser = (User)_context.Users.Where(i => i.Id == Id);

            _context.Dispose();
            return View(editUser);
        }

        public IActionResult Save(User user)
        {
            User editUser = (User)_context.Users.Where(i => i.Id == user.Id);

            editUser.Name = user.Name;
            editUser.LastName = user.LastName;
            editUser.Email = user.Email;

            _context.SaveChanges();
            _context.Dispose();
            return Redirect("Index");
        }
        public IActionResult Details(int Id)
        {
            User detailsUser = (User)_context.Users.Where(i => i.Id == Id);

            _context.Dispose();
            return View(detailsUser);
        }

        public IActionResult Delete(int Id)
        {
            User deleteUser = (User)_context.Users.Where(i => i.Id == Id);

            _context.Dispose();
            return View(deleteUser);
        }

        public IActionResult Remove(User user)
        {
            User deleteUser = (User)_context.Users.Where(i => i.Id == user.Id);
            LoginData loginData = (LoginData)_context.LoginData.Where(i => i.Id_User == user.Id);

            if (deleteUser != null && loginData != null)
            {
                _context.Users.Remove(deleteUser);
                _context.LoginData.Remove(loginData);
                _context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Nie można usunąć użytkownika. Brak danych w bazie.");
            }

            _context.Dispose();
            return RedirectToAction("Index");
        }
    }
}
