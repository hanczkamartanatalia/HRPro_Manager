﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Website.Database;
using Website.Entities;
using Website.Models;
using Website.Models.Account;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using NuGet.DependencyResolver;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Position = Website.Entities.Position;
using Microsoft.Win32;

namespace Website.Controllers
{
    public class EmploymentController : Controller
    {
        private AppDbContext _context;

        public EmploymentController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                List<Employment> employments = _context.Employments
                    .Include(e => e.User)
                    .Include(e => e.Manager)
                    .Include(e => e.Position)
                    .ToList();

                return View(employments);
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
            List<User> userList = _context.Users
                    .Join(
                        _context.LoginData,
                        user => user.Id,
                        login => login.Id_User,
                        (user, login) => new { User = user, Login = login }
                        )
                    .Where(joinResult => joinResult.Login.Id_Role == 2)
                    .Select(u => new User { Id = u.User.Id, Name = u.User.Name, LastName = u.User.LastName })
                    .ToList();

            List<SelectListItem> usersListItems = userList
                    .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = $"{u.Name} {u.LastName}" })
                    .ToList();

            usersListItems.Insert(0, new SelectListItem { Value = "", Text = "-- No manager --" });

            ViewData["Id_User"] = new SelectList(usersListItems, "Value", "Text");

            List<Position> positionList = _context.Positions.ToList();

            List<SelectListItem> positionsListItems = positionList
                    .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = $"{p.Name}" })
                    .ToList();

            ViewData["Id_Position"] = new SelectList(positionsListItems, "Value", "Text");

            return View();
        }

        [HttpPost]
        public IActionResult Add(Employment employment)
        {
            try
            {
                int userId = (int)TempData["UserId"];
                User user = _context.Users.FirstOrDefault(i => i.Id == userId);
                User manager = _context.Users.FirstOrDefault(u => u.Id == employment.Id_Manager);
                Position position = _context.Positions.FirstOrDefault(p => p.Id == employment.Id_Position);

                employment.User = user;
                employment.Id_User = user.Id;
                employment.Manager = manager;
                employment.Position = position;
                employment.Id_Position = position.Id;

                if (manager != null)
                {
                    employment.Id_Manager = manager.Id;
                }
                else
                {
                    employment.Id_Manager = null;
                }

                _context.Employments.Add(employment);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ErrorViewModel errorModel = new ErrorViewModel { ErrorMessage = $"Error: {ex.Message}" };
                return View("Error", errorModel);
            }
            finally
            {
                _context.Dispose();
            }
        }

        public IActionResult Edit(int Id)
        {
            try
            {
                List<User> userList = _context.Users
                    .Join(
                        _context.LoginData,
                        user => user.Id,
                        login => login.Id_User,
                        (user, login) => new { User = user, Login = login }
                        )
                    .Where(joinResult => joinResult.Login.Id_Role == 2)
                    .Select(u => new User { Id = u.User.Id, Name = u.User.Name, LastName = u.User.LastName })
                    .ToList();

                List<SelectListItem> usersListItems = userList
                    .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = $"{u.Name} {u.LastName}" })
                    .ToList();

                usersListItems.Insert(0, new SelectListItem { Value = "", Text = "-- No manager --" });

                ViewData["Id_User"] = new SelectList(usersListItems, "Value", "Text");

                List<Position> positionList = _context.Positions.ToList();

                List<SelectListItem> positionsListItems = positionList
                    .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = $"{p.Name}" })
                    .ToList();

                ViewData["Id_Position"] = new SelectList(positionsListItems, "Value", "Text");

                Employment editEmployment = _context.Employments
                    .Include(e => e.User)
                    .Include(e => e.Manager)
                    .Include(e => e.Position)
                    .FirstOrDefault(e => e.Id == Id);
                
                editEmployment.Rate = Math.Round(editEmployment.Rate, 1);
                return View(editEmployment);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ErrorViewModel errorModel = new ErrorViewModel { ErrorMessage = $"Error: {ex.Message}" };
                return View("Error", errorModel);
            }
        }

        [HttpPost]
        public IActionResult Save(Employment employment)
        {
            try
            {
                Employment editEmployment = _context.Employments
                    .Include(e => e.User)
                    .Include(e => e.Manager)
                    .Include(e => e.Position)
                    .FirstOrDefault(e => e.Id == employment.Id);

                User user = _context.Users.FirstOrDefault(i => i.Name == employment.User.Name && i.LastName == employment.User.LastName);
                User manager = _context.Users.FirstOrDefault(u => u.Id == employment.Id_Manager);
                Position position = _context.Positions.FirstOrDefault(p => p.Id == employment.Id_Position);

                editEmployment.EmploymentDate = employment.EmploymentDate;
                editEmployment.User = user;
                editEmployment.Id_User = user.Id;
                editEmployment.Manager = manager;
                editEmployment.Position = position;
                editEmployment.Id_Position = position.Id;
                editEmployment.Rate = employment.Rate;

                if (manager != null)
                {
                    editEmployment.Id_Manager = manager.Id;
                }
                else
                {
                    editEmployment.Id_Manager = null;
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ErrorViewModel errorModel = new ErrorViewModel { ErrorMessage = $"Error: {ex.Message}" };
                return View("Error", errorModel);
            }
            finally
            {
                _context.Dispose();
            }
        }
        public IActionResult GroupAction()
        {
            return View();
        }

        public IActionResult MakeGroupAction(Employment emp)
        {
            if(emp.Rate == 0)
            {
                ModelState.AddModelError(string.Empty, "The Increase/Decrease value cannot be 0.");
                return View("GroupAction");
            }

            List<Employment> employments = _context.Employments
                    .Include(e => e.User)
                    .Include(e => e.Manager)
                    .Include(e => e.Position)
                    .ToList(); 
            
            foreach (Employment employment in employments)
            {
                employment.Rate += emp.Rate;
                
                if (employment.Rate < 0)
                {
                    employment.Rate = 0;
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int Id)
        {
            try
            {
                Employment employment = _context.Employments
                    .Include(e => e.User)
                    .Include(e => e.Manager)
                    .Include(e => e.Position)
                    .FirstOrDefault(e => e.Id == Id);

                return View(employment);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ErrorViewModel errorModel = new ErrorViewModel { ErrorMessage = $"Error: {ex.Message}" };
                return View("Error", errorModel);
            }
            finally
            {
                _context.Dispose();
            }
        }
    }
}
