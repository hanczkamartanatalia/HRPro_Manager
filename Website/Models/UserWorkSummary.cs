namespace Website.Models
{
    public class UserWorkSummary
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public decimal TotalHours { get; set; }
        public decimal Earnings { get; set; } 
    }
}
