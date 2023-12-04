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
            return View();
        }
        public IActionResult Add(Employment employment)
        {
            int userId = (int)TempData["UserId"];
            User user = _context.Users.FirstOrDefault(i => i.Id == userId);
            User manager = _context.Users.FirstOrDefault(u => u.Name == employment.Manager.Name && u.LastName == employment.Manager.LastName);
            Position position = _context.Positions.FirstOrDefault(p => p.Name == employment.Position.Name);

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
            User manager = _context.Users.FirstOrDefault(u => u.Name == employment.Manager.Name && u.LastName == employment.Manager.LastName);
            Position position = _context.Positions.FirstOrDefault(p => p.Name == employment.Position.Name);

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
