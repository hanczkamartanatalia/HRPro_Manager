using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Database;
using Website.Entities;
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
            var userId = HttpContext.Session.GetInt32("U_Id");
            var roleName = HttpContext.Session.GetString("R_Name");

            try
            {
                
                    var query = from user in _context.Users
                                join employment in _context.Employments on user.Id equals employment.Id_User
                                join application in _context.Applications on user.Id equals application.Id_User
                                join category in _context.Categories on application.Id_Category equals category.Id
                                where employment.Id_Manager == userId
                                where application.Id_Category == 1
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
            catch (Exception ex)
            {
                return View("Home"); 
            }
        }


        public ActionResult Accept(int id)
        {
            try
            {
                var recordToEdit = _context.Applications.Find(id);

                if (recordToEdit == null)
                {
                    return NotFound();
                }

                recordToEdit.Id_Category = 3;
                recordToEdit.ChangeCatDate = DateTime.Now;


                _context.SaveChanges();

                return RedirectToAction("Archives");
            }
            catch (Exception ex)
            {

                return View("Index"); 
            }
        }

        public ActionResult Reject(int id)
        {
            try
            {
                var recordToEdit = _context.Applications.Find(id);

                if (recordToEdit == null)
                {
                    return NotFound();
                }

                recordToEdit.Id_Category = 2;
                recordToEdit.ChangeCatDate = DateTime.Now;

                _context.SaveChanges();

                return RedirectToAction("Archives");
            }
            catch (Exception ex)
            {
                return View("Index");
            }
        }

        public IActionResult Archives()
        {
            var userId = HttpContext.Session.GetInt32("U_Id");
            var roleName = HttpContext.Session.GetString("R_Name");

            try
            {
                if (roleName == "Manager")
                {
                    var query = from user in _context.Users
                                join employment in _context.Employments on user.Id equals employment.Id_User
                                join application in _context.Applications on user.Id equals application.Id_User
                                join category in _context.Categories on application.Id_Category equals category.Id
                                where employment.Id_Manager == userId
                                where application.Id_Category != 1
                                select new Approve
                                {
                                    User = user,
                                    Employment = employment,
                                    Application = application,
                                    Category = category
                                };
                    var sortedQuery = query.OrderByDescending(approve => approve.Application.ChangeCatDate);

                    var result = sortedQuery.ToList();
                    return View(result);


                }
                if (roleName == "Admin")
                {
                    var query = from user in _context.Users
                                join employment in _context.Employments on user.Id equals employment.Id_User
                                join application in _context.Applications on user.Id equals application.Id_User
                                join category in _context.Categories on application.Id_Category equals category.Id
                                where application.Id_Category != 1
                                select new Approve
                                {
                                    User = user,
                                    Employment = employment,
                                    Application = application,
                                    Category = category
                                };
                    var sortedQuery = query.OrderByDescending(approve => approve.Application.ChangeCatDate);

                    var result = sortedQuery.ToList();
                    return View(result);

                }
                else
                {
                    return View("Home");
                }

            }

            catch (Exception ex)
            {
                return View("Home");
            }


        }
        public IActionResult AllApplications()
        {
            try
            {
                var query = from user in _context.Users
                            join employment in _context.Employments on user.Id equals employment.Id_User
                            join application in _context.Applications on user.Id equals application.Id_User
                            join category in _context.Categories on application.Id_Category equals category.Id
                            where application.Id_Category == 1
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

            catch (Exception ex)
            {
                return View("Home");
            }


        }

    }
}
