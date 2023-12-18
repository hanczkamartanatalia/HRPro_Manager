using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Website.Database;
using Website.Entities;
using Website.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Website.Controllers
{
    public class ReportController : Controller
    {
        private AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        public IActionResult Index()
        {
            try
            {
                var usersTiems = GetUsersItems();

                ViewData["Id_User"] = new SelectList(usersTiems, "Value", "Text");
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ErrorViewModel errorModel = new ErrorViewModel { ErrorMessage = $"Error: {ex.Message}" };
                return View("Error", errorModel);
            }
        }

        public IActionResult GenerateSummaryReport(string summaryMonth)
        {
            try
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

                if (summaryReport.Count == 0)
                {
                    ModelState.AddModelError("summaryMonth", "No working hours for the selected period.");
                    return View("Index");
                }

                ViewBag.SummaryReport = summaryReport;
                ViewBag.StartDate = startDate.ToString("MMMM yyyy");

                return View("SummaryReport", summaryReport);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ErrorViewModel errorModel = new ErrorViewModel { ErrorMessage = $"Error: {ex.Message}" };
                return View("Error", errorModel);
            }
        }

        [HttpPost]
        public IActionResult GenerateIndividualReport(User user, string individualMonth)
        {
            try
            {
                DateTime startDate = DateTime.Parse(individualMonth);
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
                
                if (employment == null)
                {
                    ModelState.AddModelError("individualMonth", "User does not have employment.");
                    var usersTiems = GetUsersItems();

                    ViewData["Id_User"] = new SelectList(usersTiems, "Value", "Text");
                    return View("Index");
                }

                decimal totalHours = workTimes.Sum(wt => wt.WorkingHours);
                decimal hourlyRate = employment.Rate;
                decimal earnings = totalHours * hourlyRate;

                if (workTimes.Count == 0)
                {
                    ModelState.AddModelError("individualMonth", "No working hours for the selected period.");
                    var usersTiems = GetUsersItems();

                    ViewData["Id_User"] = new SelectList(usersTiems, "Value", "Text");
                    return View("Index");
                }

                ViewBag.TotalHours = totalHours;
                ViewBag.Earnings = earnings;
                ViewBag.UserName = $"{userWorkTime.Name} {userWorkTime.LastName}";
                ViewBag.StartDate = startDate.ToString("MMMM yyyy");

                return View("IndividualReport", workTimes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ErrorViewModel errorModel = new ErrorViewModel { ErrorMessage = $"Error: {ex.Message}" };
                return View("Error", errorModel);
            }
        }

        public IActionResult GenerateUserReport(string userSummaryMonth)
        {
            try
            {
                DateTime startDate = DateTime.Parse(userSummaryMonth);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                LoginData login = _context.LoginData.FirstOrDefault(u => u.Id == HttpContext.Session.GetInt32("LD_Id"));
                User userWorkTime = _context.Users.FirstOrDefault(u => u.Id == login.Id_User);

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
                
                if (employment == null)
                {
                    Console.WriteLine($"Error: User does not have employment.");
                    ErrorViewModel errorModel = new ErrorViewModel { ErrorMessage = $"Error: User does not have employment." };
                    return View("Error", errorModel);
                }

                decimal totalHours = workTimes.Sum(wt => wt.WorkingHours);
                decimal hourlyRate = employment.Rate;
                decimal earnings = totalHours * hourlyRate;

                if (workTimes.Count == 0)
                {
                    Console.WriteLine($"Error: No working hours for the selected period.");
                    ErrorViewModel errorModel = new ErrorViewModel { ErrorMessage = $"Error: No working hours for the selected period." };
                    return View("Error", errorModel);
                }

                ViewBag.TotalHours = totalHours;
                ViewBag.Earnings = earnings;
                ViewBag.UserName = $"{userWorkTime.Name} {userWorkTime.LastName}";
                ViewBag.StartDate = startDate.ToString("MMMM yyyy");

                return View("UserReport", workTimes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ErrorViewModel errorModel = new ErrorViewModel { ErrorMessage = $"Error: {ex.Message}" };
                return View("Error", errorModel);
            }
        }
        private List<SelectListItem> GetUsersItems()
        {
            List<User> userList = _context.Users.ToList();

            List<SelectListItem> usersListItems = userList
                .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = $"{u.Name} {u.LastName}" })
                .ToList();

            return usersListItems;
        }
    }
}
