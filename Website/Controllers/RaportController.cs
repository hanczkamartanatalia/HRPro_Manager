using Microsoft.AspNetCore.Mvc;
using Website.Database;
using Website.Entities;
using Website.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Website.Controllers
{
    public class RaportController : Controller
    {
        private AppDbContext _context;

        public RaportController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GenerateSummaryReport(string summaryMonth)
        {
            DateTime startDate = DateTime.Parse(summaryMonth);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            var summaryReport = _context.WorkTimes
    .Where(wt => wt.WorkingDay >= startDate && wt.WorkingDay <= endDate)
    .Join(
        _context.Employments,
        workTime => workTime.Id_User,
        employment => employment.Id_User,
        (workTime, employment) => new UserWorkSummary
        {
            UserId = workTime.Id_User,
            Name = workTime.User.Name,
            Lastname = workTime.User.LastName,
            TotalHours = workTime.WorkingHours,
            Earnings = employment.Rate * workTime.WorkingHours
        })
    .ToList();


            ViewBag.SummaryReport = summaryReport;

            return View("SummaryReport", summaryReport);
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

            return View("IndividualReport", workTimes);
        }
    }
}
