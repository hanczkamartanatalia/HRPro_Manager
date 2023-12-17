using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Website.Database;
using Website.Entities;
using Website.Models;
using Website.Models.Account;

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
            List<User> userList = _context.Users.ToList();

            List<SelectListItem> usersListItems = userList
                .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = $"{u.Name} {u.LastName}" })
                .ToList();

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
                employment.Id_Manager = manager.Id;
                employment.Position = position;
                employment.Id_Position = position.Id;

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
                List<User> userList = _context.Users.ToList();

                List<SelectListItem> usersListItems = userList
                    .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = $"{u.Name} {u.LastName}" })
                    .ToList();

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
                editEmployment.Id_Manager = manager.Id;
                editEmployment.Position = position;
                editEmployment.Id_Position = position.Id;
                editEmployment.Rate = employment.Rate;

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
