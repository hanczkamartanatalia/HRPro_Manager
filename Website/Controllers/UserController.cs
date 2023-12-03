using Microsoft.AspNetCore.Mvc;
using Website.Database;
using Website.Entities;

namespace Website.Controllers
{
    public class UserController : Controller
    {
        private  AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<User> users = _context.Users.ToList();
            return View(users);
        }

        public IActionResult Create() 
        {
            return View();
        }

        public IActionResult Add(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                _context.Dispose();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Create");
        }

        public IActionResult Edit(int Id) 
        {
            User editUser = _context.Users.Where(i => i.Id == Id).FirstOrDefault();

            _context.Dispose();
            return View(editUser);
        }

        public IActionResult Save(User user)
        {
            User editUser = _context.Users.Where(i => i.Id == user.Id).FirstOrDefault();

            editUser.Name = user.Name;
            editUser.LastName = user.LastName;
            editUser.Email = user.Email;

            _context.SaveChanges();
            _context.Dispose();
            return Redirect("Index");
        }
        public IActionResult Details(int Id)
        {
            User detailsUser = _context.Users.Where(i => i.Id == Id).FirstOrDefault();

            _context.Dispose();
            return View(detailsUser);
        }

        public IActionResult Delete(int Id)
        {
            User deleteUser = _context.Users.Where(i => i.Id == Id).FirstOrDefault();

            _context.Dispose();
            return View(deleteUser);
        }

        public IActionResult Remove(User user)
        {
            User deleteUser = _context.Users.Where(i => i.Id == user.Id).FirstOrDefault();

            _context.Users.Remove(deleteUser);
            _context.SaveChanges();
            _context.Dispose();
            return Redirect("index");
        }
    }

}
