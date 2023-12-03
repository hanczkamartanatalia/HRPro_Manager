using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Website.Database;
using Website.Entities;
using Website.Models.Account;
using Website.Models.Employment;

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
            List<EmploymentModel> employmentModels = _context.Employments
                .Include(e => e.User)
                .Include(e => e.Manager)
                .Include(e => e.Position)
                .Select(e => new EmploymentModel
                {
                    Id = e.Id,
                    EmploymentDate = e.EmploymentDate,
                    Rate = e.Rate,
                    User = new User { Name = e.User.Name, LastName = e.User.LastName, Email = e.User.Email },
                    Manager = new User { Name = e.Manager.Name, LastName = e.Manager.LastName, Email = e.Manager.Email },
                    Position = new Position { Name = e.Position.Name } // Zakładam, że Position ma właściwość Name
                })
                .ToList();

            return View(employmentModels);
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Add(EmploymentModel employment)
        {
            int userId = (int)TempData["UserId"];
            User user = _context.Users.FirstOrDefault(i => i.Id == userId);
            User manager = _context.Users.FirstOrDefault(u => u.Name == employment.Manager.Name && u.LastName == employment.Manager.LastName);
            Position position = _context.Positions.FirstOrDefault(p => p.Name == employment.Position.Name);
            Employment employment1 = new Employment
            {
                EmploymentDate = employment.EmploymentDate,
                User = user,
                Id_User = user.Id,
                Manager = manager,
                Id_Manager = manager.Id,
                Position = position,
                Id_Position = position.Id,
                Rate = employment.Rate
            };

            _context.Employments.Add(employment1);
            _context.SaveChanges();
            _context.Dispose();
            return RedirectToAction("Index");
        }
    }
}
