using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Website.Database;
using Website.Entities;
using Website.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Website.Controllers
{
    public class ReportController : Controller
    {
        private AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
            
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
        }

        public IActionResult Index()
        {
            List<User> userList = _context.Users.ToList();

            List<SelectListItem> usersListItems = userList
                .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = $"{u.Name} {u.LastName}" })
                .ToList();

            ViewData["Id_User"] = new SelectList(usersListItems, "Value", "Text");
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
        .GroupBy(userWorkSummary => new { userWorkSummary.UserId, userWorkSummary.Name, userWorkSummary.Lastname })
        .Select(group => new UserWorkSummary
        {
            UserId = group.Key.UserId,
            Name = group.Key.Name,
            Lastname = group.Key.Lastname,
            TotalHours = group.Sum(userWorkSummary => userWorkSummary.TotalHours),
            Earnings = group.Sum(userWorkSummary => userWorkSummary.Earnings)
        })
        .ToList();


            ViewBag.SummaryReport = summaryReport;
            ViewBag.SummaryReport = summaryReport;
            ViewBag.StartDate = startDate.ToString("MMMM yyyy");

            return View("SummaryReport", summaryReport);
        }

        [HttpPost]
        public IActionResult GenerateIndividualReport(User user, string summaryMonth)
        {
            DateTime startDate = DateTime.Parse(summaryMonth);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            User userWorkTime = _context.Users.FirstOrDefault(u => u.Id == user.Id);

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

 
            Employment employment = _context.Employments.FirstOrDefault(e => e.Id_User == userWorkTime.Id);
            
            decimal totalHours = workTimes.Sum(wt => wt.WorkingHours);
            decimal hourlyRate = employment.Rate; 
            decimal earnings = totalHours * hourlyRate;


            ViewBag.TotalHours = totalHours;
            ViewBag.Earnings = earnings; 
            ViewBag.UserName = $"{userWorkTime.Name} {userWorkTime.LastName}";
            ViewBag.StartDate = startDate.ToString("MMMM yyyy");

            return View("IndividualReport", workTimes);
        }
    }
}
