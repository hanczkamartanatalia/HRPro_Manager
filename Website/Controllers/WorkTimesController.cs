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
            var appDbContext = _context.WorkTimes.Include(w => w.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: WorkTimes/Details/5
        public async Task<IActionResult> Details(int? id)
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
            var workTime = new WorkTime
            {
                Id_User = 5, //tutaj dodac id usera, który jest zalogowany
                WorkingDay = WorkingDay,
                WorkingHours = WorkingHours,
            };

            _context.Add(workTime);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: WorkTimes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_User,WorkingDay,WorkingHours,Id")] WorkTime workTime)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workTime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id_User"] = new SelectList(_context.Users, "Id", "Id", workTime.Id_User);
            return View(workTime);
        }

        // GET: WorkTimes/Edit/5
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
            ViewData["Id_User"] = new SelectList(_context.Users, "Id", "Id", workTime.Id_User);
            return View(workTime);
        }

        // POST: WorkTimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_User,WorkingDay,WorkingHours,Id")] WorkTime workTime)
        {
            if (id != workTime.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
            ViewData["Id_User"] = new SelectList(_context.Users, "Id", "Id", workTime.Id_User);
            return View(workTime);
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
            if (_context.WorkTimes == null)
            {
                return Problem("Entity set 'AppDbContext.WorkTimes'  is null.");
            }
            var workTime = await _context.WorkTimes.FindAsync(id);
            if (workTime != null)
            {
                _context.WorkTimes.Remove(workTime);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkTimeExists(int id)
        {
          return (_context.WorkTimes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
