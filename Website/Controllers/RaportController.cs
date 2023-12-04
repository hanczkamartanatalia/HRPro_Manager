using Microsoft.AspNetCore.Mvc;
using Website.Database;
using Website.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Website.Controllers
{
    public class RaportController : Controller
    {
        private  AppDbContext _context;

        public RaportController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GenerateSummaryReport()
        {

            return View("SummaryReport");
        }

        [HttpPost]
        public IActionResult GenerateIndividualReport(User user, string summaryMonth)
        {
            DateTime startDate = DateTime.Parse(summaryMonth);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            User userWorkTime = _context.Users.FirstOrDefault(u => u.Name == user.Name && u.LastName == user.LastName);
            
            List<WorkTime> workTimes = _context.WorkTimes
                .Where(wt => wt.Id_User == userWorkTime.Id && wt.WorkingDay >= startDate && wt.WorkingDay <= endDate)
                .OrderBy(wt => wt.WorkingDay)
                .Select(wt => new WorkTime
                {
                    Id_User = wt.Id_User,
                    User = wt.User,
                    WorkingDay = wt.WorkingDay,
                    WorkingHours = wt.WorkingHours
                })
                .ToList();

            decimal totalHours = workTimes.Sum(wt => wt.WorkingHours);

            ViewBag.TotalHours = totalHours;
            ViewBag.UserName = $"{userWorkTime.Name} {userWorkTime.LastName}";

            return View("GenerateIndividualReport", workTimes);
        }
    }
}
