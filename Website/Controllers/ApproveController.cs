using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Database;
using Website.Models;

namespace Website.Controllers
{
    public class ApproveController : Controller
    {
        private readonly AppDbContext _context;

        public ApproveController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            
            var query = from user in _context.Users
                        join employment in _context.Employments on user.Id equals employment.Id_User
                        join application in _context.Applications on user.Id equals application.Id_User
                        join category in _context.Categories on application.Id_Category equals category.Id
                        where employment.Id_Manager == 10 // tutaj dopisac ig zalogowanego usera
                        where application.Id_Category == 1
                        select new Approve
                        {/*
                            UserName = user.Name,
                            UserLastName = user.LastName,
                            ManagerName = _context.Users
                                .Where(u => u.Id == employment.Id_Manager)
                                .Select(u => u.Name + " " + u.LastName)
                                .FirstOrDefault(),
                            StartDate = application.StartDate,
                            EndDate = application.EndDate,
                            CategoryName = category.Name
                            */
                            User = user,
                            Employment = employment,
                            Application = application,
                            Category = category
                        };
         
            var result = query.ToList();

            return View(result);
          
        }

        public ActionResult Accept(int id)
        {
            var recordToEdit = _context.Applications.Find(id);
            recordToEdit.Id_Category = 3; 
            
            _context.SaveChanges();
            return RedirectToAction("Archives");
        }

        public ActionResult Reject(int id)
        {
            var recordToEdit = _context.Applications.Find(id);
            recordToEdit.Id_Category = 2;

            _context.SaveChanges();
            return RedirectToAction("Archives");
        }

        public IActionResult Archives()
        {

            var query = from user in _context.Users
                        join employment in _context.Employments on user.Id equals employment.Id_User
                        join application in _context.Applications on user.Id equals application.Id_User
                        join category in _context.Categories on application.Id_Category equals category.Id
                        where employment.Id_Manager == 10 // tutaj dopisac ig zalogowanego usera
                        where application.Id_Category != 1
                        select new Approve
                        {
                            User = user,
                            Employment = employment,
                            Application = application,
                            Category = category
                        };

            var result = query.ToList();

            return View(result);

        }

    }
}
