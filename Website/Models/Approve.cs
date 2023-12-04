using System.ComponentModel.DataAnnotations;
using Website.Entities;
using Website.Models.Account;
namespace Website.Models

{
    public class Approve
    {
        public User User { get; set; }
        public Employment Employment { get; set; }
        public Category Category { get; set; }
        public Application Application { get; set; }
        public string UserName { get; internal set; }
        public string UserLastName { get; internal set; }
        public string? ManagerName { get; internal set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; internal set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; internal set; }
        public string CategoryName { get; internal set; }
    }
}
