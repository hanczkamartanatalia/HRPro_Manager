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
    public class WorkTimesController : Controller
    {
        private readonly AppDbContext _context;

        public WorkTimesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: WorkTimes
        public async Task<IActionResult> Index()
        {
            try
            {
                var appDbContext = _context.WorkTimes.Include(w => w.User);
                var workTimes = await appDbContext.ToListAsync();

                if (workTimes == null)
                {
                    return NotFound(); 
                }

                return View(workTimes);
            }
            catch (Exception ex)
            {
                return View("Home"); 
            }
        }
        public async Task<IActionResult> IndexUser()
        {
            var userId = HttpContext.Session.GetInt32("LD_Id");
            var userWorkTimes = await _context.WorkTimes
                .Where(w => w.Id_User == userId)
                .ToListAsync();

            return View(userWorkTimes);
        }

         // GET: WorkTimes/Create for admin and manager 
        public IActionResult Create()
        {
           
            List<User> userList = _context.Users.ToList();

            List<SelectListItem> usersListItems = userList
                .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = $"{u.Name} {u.LastName}" })
                .ToList();

            ViewData["Id_User"] = new SelectList(usersListItems, "Value", "Text");

            return View();
        }
        public IActionResult CreateUser()
        {
            return View();
        }
        //WorkTimes/createUser for user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(DateTime WorkingDay, decimal WorkingHours)
        {
            try
            {
                if (WorkingDay == default(DateTime) || WorkingHours <= 0)
                {
                    ModelState.AddModelError(string.Empty, "Invalid input parameters.");
                    return View();
                }

                // Check if the WorkingDay is in the future
                if (WorkingDay > DateTime.Now)
                {
                    ModelState.AddModelError(string.Empty, "Working day cannot be in the future.");
                    return View(); 
                }
                var userId = HttpContext.Session.GetInt32("LD_Id");
                var workTime = new WorkTime
                {
                    Id_User = (int)userId,
                    WorkingDay = WorkingDay,
                    WorkingHours = WorkingHours,
                };

                _context.Add(workTime);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(IndexUser));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                return View(); 
            }
        }


        // POST: WorkTimes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_User,WorkingDay,WorkingHours")] WorkTime workTime)
        {
            try
            {
                _context.Add(workTime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
               
            }
            catch (Exception ex)
            {
                ViewData["Id_User"] = new SelectList(_context.Users, "Id", "Id", workTime.Id_User);
                return View(workTime); 
            }
        }

        // GET: WorkTimes/Edit/
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.WorkTimes == null)
            {
                return NotFound();
            }

            var workTime = await _context.WorkTimes.FindAsync(id);
            if (workTime == null)
            {
                return NotFound();
            }
            List<User> userList = _context.Users.ToList();

            List<SelectListItem> usersListItems = userList
                .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = $"{u.Name} {u.LastName}" })
                .ToList();

            ViewData["Id_User"] = new SelectList(usersListItems, "Value", "Text");
            return View(workTime);

        }
        public async Task<IActionResult> EditUser(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var workTime = await _context.WorkTimes.FindAsync(id);
                if (workTime == null)
                {
                    return NotFound();
                }

                ViewData["Id_User"] = new SelectList(_context.Users, "Id", "Id", workTime.Id_User);
                return View(workTime);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(IndexUser)); 
            }
        }


        // POST: WorkTimes/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_User,WorkingDay,WorkingHours,Id")] WorkTime workTime)
        {
            if (id != workTime.Id)
            {
                return NotFound();
            }

            
            try
            {
                _context.Update(workTime);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkTimeExists(workTime.Id))
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, [Bind("Id_User,WorkingDay,WorkingHours,Id")] WorkTime workTime)
        {
            if (id != workTime.Id)
            {
                return NotFound();
            }
            if (workTime.WorkingDay > DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Working day cannot be in the future.");
                return View(workTime);
            }

            try
            {
                _context.Update(workTime);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkTimeExists(workTime.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(IndexUser));

            
        }

        // GET: WorkTimes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.WorkTimes == null)
            {
                return NotFound();
            }

            var workTime = await _context.WorkTimes
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workTime == null)
            {
                return NotFound();
            }

            return View(workTime);
        }

        // POST: WorkTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (_context.WorkTimes == null)
                {
                    return Problem("Entity set 'AppDbContext.WorkTimes' is null.");
                }

                var workTime = await _context.WorkTimes.FindAsync(id);
                if (workTime == null)
                {
                    return NotFound(); 
                }

                _context.WorkTimes.Remove(workTime);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Index"); 
            }
        }

        private bool WorkTimeExists(int id)
        {
          return (_context.WorkTimes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
