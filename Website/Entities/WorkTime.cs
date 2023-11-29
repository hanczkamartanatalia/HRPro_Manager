using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.Entities
{
    public class WorkTime : Entity 
    {
        
        [ForeignKey(nameof(User))]
        public int Id_User { get; set; } = default!;

        [ForeignKey(nameof(Id_User))]
        public User User { get; set; } = default!;

        [DataType(DataType.Date)]
        public DateTime WorkingDay { get; set; } = default!;
        
        public decimal WorkingHours { get; set; } = default!;


    }
}
