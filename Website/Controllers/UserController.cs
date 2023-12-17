using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Data;
using System.Text.RegularExpressions;
using Website.Database;
using Website.Entities;
using Website.Models;
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
            try
            {
                var usersWithLoginDataAndRole = _context.Users
                .Join(
                    _context.LoginData,
                    user => user.Id,
                    login => login.Id_User,
                    (user, login) => new { User = user, Login = login }
                    )
                .Join(
                    _context.Roles,
                    userLogin => userLogin.Login.Id_Role,
                    role => role.Id,
                    (userLogin, role) => new Tuple<User, LoginData, Role>(userLogin.User, userLogin.Login, role)
                    )
                .ToList();

                return View(usersWithLoginDataAndRole);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ErrorViewModel errorModel = new ErrorViewModel { ErrorMessage = $"Error: {ex.Message}" };
                return View("Error", errorModel);
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Register register)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Create", register);
                }

                User user = new User
                {
                    Name = register.User.Name,
                    LastName = register.User.LastName,
                    Email = register.User.Email
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                Role role = _context.Roles.SingleOrDefault(i => i.Id == 3 && i.Name == "Employee");

                if (role == null)
                {
                    ModelState.AddModelError(string.Empty, "Rola o Id=3 nie istnieje w bazie danych.");
                    return View("Create", register);
                }

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
                TempData["UserId"] = user.Id;
                return RedirectToAction("Create", "Employment");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ErrorViewModel errorModel = new ErrorViewModel { ErrorMessage = $"Error: {ex.Message}" };
                return View("Error", errorModel);
            }
        }

        public IActionResult Edit(int Id)
        {
            try
            {
                User editUser = _context.Users.FirstOrDefault(i => i.Id == Id);

                _context.Dispose();
                return View(editUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ErrorViewModel errorModel = new ErrorViewModel { ErrorMessage = $"Error: {ex.Message}" };
                return View("Error", errorModel);
            }

        }

        public IActionResult Save(User user)
        {
            try
            {
                User editUser = _context.Users.FirstOrDefault(i => i.Id == user.Id);

                editUser.Name = user.Name;
                editUser.LastName = user.LastName;
                editUser.Email = user.Email;

                _context.SaveChanges();
                _context.Dispose();
                return Redirect("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ErrorViewModel errorModel = new ErrorViewModel { ErrorMessage = $"Error: {ex.Message}" };
                return View("Error", errorModel);
            }
        }

        public IActionResult Details(int Id)
        {
            try
            {
                User detailsUser = _context.Users.FirstOrDefault(i => i.Id == Id);

                _context.Dispose();
                return View(detailsUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ErrorViewModel errorModel = new ErrorViewModel { ErrorMessage = $"Error: {ex.Message}" };
                return View("Error", errorModel);
            }
        }

        public IActionResult Delete(int Id)
        {
            try
            {
                User deleteUser = _context.Users.FirstOrDefault(i => i.Id == Id);

                _context.Dispose();
                return View(deleteUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ErrorViewModel errorModel = new ErrorViewModel { ErrorMessage = $"Error: {ex.Message}" };
                return View("Error", errorModel);
            }
        }

        public IActionResult Remove(User user)
        {
            try
            {
                User deleteUser = _context.Users.FirstOrDefault(i => i.Id == user.Id);
                LoginData loginData = _context.LoginData.FirstOrDefault(i => i.Id_User == user.Id);
                List<Employment> employments = _context.Employments
                    .Where(e => e.Id_User == user.Id)
                    .ToList();

                if (deleteUser != null && loginData != null && employments != null)
                {
                    _context.Users.Remove(deleteUser);
                    _context.LoginData.Remove(loginData);
                    _context.Employments.RemoveRange(employments);
                    _context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Cannot delete the user. No data in the database.");
                }

                _context.Dispose();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ErrorViewModel errorModel = new ErrorViewModel { ErrorMessage = $"Error: {ex.Message}" };
                return View("Error", errorModel);
            }
        }
        public IActionResult ChangeLogin(int Id)
        {
            try
            {
                LoginData editLoginData = _context.LoginData.FirstOrDefault(i => i.Id_User == Id);
                editLoginData.Password = string.Empty;

                return View(editLoginData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ErrorViewModel errorModel = new ErrorViewModel { ErrorMessage = $"Error: {ex.Message}" };
                return View("Error", errorModel);
            }
        }

        public IActionResult SaveLogin(LoginData loginData)
        {
            try
            {
                LoginData editLoginData = _context.LoginData.FirstOrDefault(i => i.Id_User == loginData.Id_User);

                editLoginData.Login = loginData.Login;
                editLoginData.Password = PasswordService.HashPassword(loginData.Password);

                _context.SaveChanges();
                _context.Dispose();
                return Redirect("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ErrorViewModel errorModel = new ErrorViewModel { ErrorMessage = $"Error: {ex.Message}" };
                return View("Error", errorModel);
            }
        }

        public IActionResult ChangeRole(int Id)
        {
            try
            {
                LoginData editLoginData = _context.LoginData.FirstOrDefault(i => i.Id_User == Id);

                if (editLoginData.Id_Role == 3)
                {
                    Role role = _context.Roles.SingleOrDefault(i => i.Id == 2);
                    editLoginData.Id_Role = role.Id;
                    editLoginData.Role = role;
                }
                else if (editLoginData.Id_Role == 2)
                {

                    Role role = _context.Roles.SingleOrDefault(i => i.Id == 3);
                    editLoginData.Id_Role = role.Id;
                    editLoginData.Role = role;
                }

                _context.SaveChanges();
                _context.Dispose();
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
