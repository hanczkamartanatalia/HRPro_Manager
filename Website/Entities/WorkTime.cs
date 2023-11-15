using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.Areas.Identity.Data;

namespace Website.Entities
{
    public class WorkTime 
    {
        public int Id { get; set; } = default!;



        [ForeignKey(nameof(User))]
        public int Id_User { get; set; } = default!;

        [ForeignKey(nameof(Id_User))]
        public AppUser User { get; set; } = default!;



        public DateTime WorkingDay { get; set; } = default!;
        public TimeSpan WorkingHours { get; set; } = default!;
    }
}
