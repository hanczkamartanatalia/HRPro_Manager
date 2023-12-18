using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Website.Database;
using Website.Entities;

namespace Website.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly AppDbContext _context;

        public ApplicationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Applications
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("LD_Id");
            try
            {
                var appDbContext = _context.Applications
                    .Include(a => a.Category)
                    .Include(a => a.User)
                    .Where(a => a.Id_User == userId); 

                var applications = await appDbContext.ToListAsync();

                if (applications == null)
                {
                    return NotFound(); 
                }

                return View(applications);
            }
            catch (Exception ex)
            {
                return View("Home"); 
            }
        }

  

        // GET: Applications/Create
        public IActionResult Create()
        {
            ViewData["Id_Category"] = new SelectList(_context.Categories, "Id", "Id");
            ViewData["Id_User"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Applications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DateTime StartDate, DateTime EndDate)
        {
            var userId = HttpContext.Session.GetInt32("LD_Id");

            try
            {
                // Validate input parameters
                if (StartDate > EndDate)
                {
                    ModelState.AddModelError(string.Empty, "Start date cannot be later than end date.");
                    return View(); 
                }

                var application = new Application
                {
                    Id_User = (int)userId,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    Id_Category = 1
                };

                _context.Add(application);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Index"); 
            }
        }

        // GET: Applications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Applications == null)
            {
                return NotFound();
            }

            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            ViewData["Id_Category"] = new SelectList(_context.Categories, "Id", "Id", application.Id_Category);
            ViewData["Id_User"] = new SelectList(_context.Users, "Id", "Id", application.Id_User);
            return View(application);
        }

        // EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_User,Id_Category,StartDate,EndDate,Id")] Application application)
        {
            if (id != application.Id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(application);
                 await _context.SaveChangesAsync();
             }
             catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationExists(application.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            
         
        }

        // GET: Applications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Applications == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .Include(a => a.Category)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try 
            {
                if (_context.Applications == null)
                {
                    return Problem("Entity set 'AppDbContext.Applications'  is null.");
                }
                var application = await _context.Applications.FindAsync(id);
                if (application != null)
                {
                    _context.Applications.Remove(application);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                return View("Index");
            }

        }

        private bool ApplicationExists(int id)
        {
          return (_context.Applications?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
