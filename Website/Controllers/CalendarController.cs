using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Database;
using Website.Service;


namespace Website.Controllers
{
    public class CalendarController : Controller
    {
        private AppDbContext _context;
        private int selectedMonth = DateTime.Now.Month;

        public CalendarController(AppDbContext context)
        {
            _context = context;
        }

        public int GetNumberOfPeopleOnLeave(DateTime date)
        {
            return _context.Applications
                .Count(a => a.StartDate <= date && a.EndDate >= date);
        }
        public ActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTime.Today;
            }

            if (!endDate.HasValue)
            {
                endDate = startDate.Value.AddDays(30);
            }

            var dates = GetDatesForCalendar(startDate, endDate);

            var numberOfPeopleOnLeave = new List<int>();

            foreach (var date in dates)
            {
                var count = GetNumberOfPeopleOnLeave(date);
                numberOfPeopleOnLeave.Add(count);
            }

            ViewBag.Dates = dates;
            ViewBag.NumberOfPeopleOnLeave = numberOfPeopleOnLeave;

            return View();
        }

        private List<DateTime> GetDatesForCalendar(DateTime? startDate, DateTime? endDate)
        {
            startDate ??= DateTime.Today;
            endDate ??= startDate.Value.AddDays(100);

            var dates = new List<DateTime>();
            for (var date = startDate.Value; date <= endDate; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            return dates;
        }


        [HttpPost]
        public ActionResult GetNext30Days()
        {
            var startDate = DateTime.Now.Date.AddDays(31);
            var endDate = startDate.AddDays(30);

            var dates = new List<DateTime>();
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            return RedirectToAction("Index");
        }
    }

}

