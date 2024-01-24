using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using NuGet.Protocol.Plugins;
using System.Data;
using System.Text.RegularExpressions;
using Website.Database;
using Website.Entities;
using Website.Models;
using Website.Models.Account;
using Website.Service;
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

        public IActionResult Index(string searchName = null, string searchLastname = null)
        {

            try
            {
                List<Tuple<User, LoginData, Role>> usersWithLoginDataAndRole;

                if (!string.IsNullOrEmpty(searchName) && !string.IsNullOrEmpty(searchLastname))
                {
                    usersWithLoginDataAndRole = FindUser(searchName, searchLastname);
                }
                else
                {
                    usersWithLoginDataAndRole = GetUsersWithLoginDataAndRole();
                }

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
                User user = new User
                {
                    Name = register.User.Name,
                    LastName = register.User.LastName,
                    Email = register.User.Email
                };

                Role role = _context.Roles.SingleOrDefault(i => i.Id == 3 && i.Name == "Employee");

                if (role == null)
                {
                    ModelState.AddModelError(string.Empty, "The Employee role does not exist in the database.");
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

                _context.Users.Add(user);
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
                
                bool hasRoleOne = _context.LoginData.Any(ld => ld.Id_User == editUser.Id && ld.Id_Role == 1);

                if (hasRoleOne)
                {
                    ModelState.AddModelError(string.Empty, "Cannot edit a user with the admin role.");
                    List<Tuple<User, LoginData, Role>> usersWithLoginDataAndRole = GetUsersWithLoginDataAndRole();
                    return View("Index", usersWithLoginDataAndRole);
                }

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
                LoginData loginData = _context.LoginData.FirstOrDefault(ld => ld.Id_User == Id);

                Tuple<User, LoginData> userDetails = new Tuple<User, LoginData>(detailsUser, loginData);
                
                bool hasRoleOne = _context.LoginData.Any(ld => ld.Id_User == detailsUser.Id && loginData.Id_Role == 1);

                if (hasRoleOne)
                {
                    ModelState.AddModelError(string.Empty, "Cannot view details a user with the admin role.");
                    List<Tuple<User, LoginData, Role>> usersWithLoginDataAndRole = GetUsersWithLoginDataAndRole();
                    return View("Index", usersWithLoginDataAndRole);
                }
                _context.Dispose();
                return View(userDetails);
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

                bool hasRoleOne = _context.LoginData.Any(ld => ld.Id_User == deleteUser.Id && ld.Id_Role == 1);

                if (hasRoleOne)
                {
                    ModelState.AddModelError(string.Empty, "Cannot delete a user with the admin role.");
                    List<Tuple<User, LoginData, Role>> usersWithLoginDataAndRole = GetUsersWithLoginDataAndRole();
                    return View("Index", usersWithLoginDataAndRole);
                }

                bool isManager = _context.Employments.Any(e => e.Id_Manager == deleteUser.Id);

                if (isManager)
                {
                    ModelState.AddModelError(string.Empty, "Cannot delete a user who has a subordinate.");
                    List<Tuple<User, LoginData, Role>> usersWithLoginDataAndRole = GetUsersWithLoginDataAndRole();
                    return View("Index", usersWithLoginDataAndRole);
                }

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
                    ModelState.AddModelError(string.Empty, "Something has gone wrong.");
                    return View(user);
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

                if (editLoginData.Id_Role == 1)
                {
                    ModelState.AddModelError(string.Empty, "Cannot change the login data of an administrator.");
                    List<Tuple<User, LoginData, Role>> usersWithLoginDataAndRole = GetUsersWithLoginDataAndRole();
                    return View("Index", usersWithLoginDataAndRole);
                }

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
                return RedirectToAction("Index");
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

                if (editLoginData.Id_Role == 1)
                {
                    ModelState.AddModelError(string.Empty, "Cannot change the role of an administrator.");
                    List<Tuple<User, LoginData, Role>> usersWithLoginDataAndRole = GetUsersWithLoginDataAndRole();
                    return View("Index", usersWithLoginDataAndRole);
                }

                if (editLoginData.Id_Role == 3)
                {
                    Role role = _context.Roles.SingleOrDefault(i => i.Id == 2);
                    editLoginData.Id_Role = role.Id;
                    editLoginData.Role = role;
                    Employment employment = _context.Employments.FirstOrDefault(i => i.Id_User == editLoginData.Id_User);
                    employment.Id_Manager = null;
                }
                else if (editLoginData.Id_Role == 2)
                {
                    bool isManager = _context.Employments.Any(e => e.Id_Manager == Id);

                    if (isManager)
                    {
                        ModelState.AddModelError(string.Empty, "Cannot delete a user who has a subordinate.");
                        List<Tuple<User, LoginData, Role>> usersWithLoginDataAndRole = GetUsersWithLoginDataAndRole();
                        return View("Index", usersWithLoginDataAndRole);
                    }

                    Role role = _context.Roles.SingleOrDefault(i => i.Id == 3);
                    editLoginData.Id_Role = role.Id;
                    editLoginData.Role = role;
                    Employment employment = _context.Employments.FirstOrDefault(i => i.Id_User == editLoginData.Id_User);
                    employment.Id_Manager = _context.LoginData.FirstOrDefault(i => i.Id_Role == 2).Id_User;
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
        public IActionResult ChangeAdmin(int Id)
        {
            try
            {
                LoginData editLoginData = _context.LoginData.FirstOrDefault(i => i.Id_User == Id);


                if (editLoginData.Id_Role == 1)
                {
                    Role role = _context.Roles.SingleOrDefault(i => i.Id == 2);
                    editLoginData.Id_Role = role.Id;
                    editLoginData.Role = role;
                }
                else if (editLoginData.Id_Role == 2 || editLoginData.Id_Role == 3)
                {

                    Role role = _context.Roles.SingleOrDefault(i => i.Id == 1);
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
        
        private List<Tuple<User, LoginData, Role>> GetUsersWithLoginDataAndRole()
        {
            List<Tuple<User, LoginData, Role>> usersWithLoginDataAndRole = _context.Users
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

            return usersWithLoginDataAndRole;
        }
        public List<Tuple<User, LoginData, Role>> FindUser(string name, string lastname)
        {
            List<Tuple<User, LoginData, Role>> findUsers = _context.Users
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
                .AsEnumerable()
                    .Where(t => t.Item1.Name.ToLower().Contains(name.ToLower()) && t.Item1.LastName.ToLower().Contains(lastname.ToLower()))

                .Select(t => Tuple.Create(t.Item1, t.Item2, t.Item3))
                .ToList();

            if (findUsers.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
            }

            return findUsers;
        }
    }
}
