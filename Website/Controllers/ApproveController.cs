﻿using Microsoft.AspNetCore.Mvc;
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
            try
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
