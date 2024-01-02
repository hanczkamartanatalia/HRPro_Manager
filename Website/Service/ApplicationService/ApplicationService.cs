using Website.Database;

namespace Website.Service
{
    public class ApplicationService
    {
        public readonly AppDbContext _context;

        public ApplicationService(AppDbContext context)
        {
            _context = context;
        }

        public int GetNumberOfPeopleOnLeave(DateTime date)
        {
            return _context.Applications
                .Count(a => a.StartDate <= date && a.EndDate >= date);
        }
    }
}
