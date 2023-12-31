﻿using Microsoft.AspNetCore.Identity;
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

            Role role = _context.Roles.SingleOrDefault(i => i.Id == 3 && i.Name == "Employee");

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
            TempData["UserId"] = user.Id;
            return RedirectToAction("Create", "Employment");

            //}

            //return View("Create");
        }

        public IActionResult Edit(int Id)
        {
            User editUser = _context.Users.FirstOrDefault(i => i.Id == Id);

            _context.Dispose();
            return View(editUser);
        }

        public IActionResult Save(User user)
        {
            User editUser = _context.Users.FirstOrDefault(i => i.Id == user.Id);

            editUser.Name = user.Name;
            editUser.LastName = user.LastName;
            editUser.Email = user.Email;

            _context.SaveChanges();
            _context.Dispose();
            return Redirect("Index");
        }

        public IActionResult Details(int Id)
        {
            User detailsUser = _context.Users.FirstOrDefault(i => i.Id == Id);

            _context.Dispose();
            return View(detailsUser);
        }

        public IActionResult Delete(int Id)
        {
            User deleteUser = _context.Users.FirstOrDefault(i => i.Id == Id);

            _context.Dispose();
            return View(deleteUser);
        }

        public IActionResult Remove(User user)
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
                Console.WriteLine("Nie można usunąć użytkownika. Brak danych w bazie.");
            }

            _context.Dispose();
            return RedirectToAction("Index");
        }
        public IActionResult ChangeLogin(int Id)
        {
            LoginData editLoginData = _context.LoginData.FirstOrDefault(i => i.Id_User == Id);
            editLoginData.Password = string.Empty;

            return View(editLoginData);
        }

        public IActionResult SaveLogin(LoginData loginData)
        {
            LoginData editLoginData = _context.LoginData.FirstOrDefault(i => i.Id_User == loginData.Id_User);

            editLoginData.Login = loginData.Login;
            editLoginData.Password = PasswordService.HashPassword(loginData.Password);

            _context.SaveChanges();
            _context.Dispose();
            return Redirect("Index");
        }

        public IActionResult ChangeRole(int Id)
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
    }
}
