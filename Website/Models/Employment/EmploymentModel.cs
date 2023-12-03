using Website.Entities;

namespace Website.Models.Employment
{
    public class EmploymentModel
    {
        public int Id { get; set; }
        public DateTime EmploymentDate { get; set; }
        public User User { get; set; } = default!;
        public User Manager { get; set; }
        public int ManagerId { get; set; }
        public Position Position { get; set; }
        public int PositionId { get; set; }
        public decimal Rate { get; set; }

        public string FormattedEmploymentDate => EmploymentDate.ToShortDateString();

    }
}
