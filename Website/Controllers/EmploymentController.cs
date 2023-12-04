using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using Website.Database;
using Website.Entities;
using Website.Models.Account;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Position = Website.Entities.Position;

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
            List<Employment> employments = _context.Employments
                .Include(e => e.User)
                .Include(e => e.Manager)
                .Include(e => e.Position)
                .ToList();

            return View(employments);
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
        public IActionResult Add(Employment employment)
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
            _context.Dispose();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int Id)
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
        public IActionResult Save(Employment employment)
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
            _context.Dispose();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int Id)
        {
            Employment employment = _context.Employments
                .Include(e => e.User)
                .Include(e => e.Manager)
                .Include(e => e.Position)
                .FirstOrDefault(e => e.Id == Id);

            _context.Dispose();
            return View(employment);
        }
    }
}
