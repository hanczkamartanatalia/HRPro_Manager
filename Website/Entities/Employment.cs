using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.Areas.Identity.Data;

namespace Website.Entities
{
    public class Employment
    {
        public int Id { get; set; } = default!;
        public DateTime EmploymentDate { get; set; } = default!;
        public decimal Rate { get; set; } = default!;



        [ForeignKey(nameof(User))]
        public int Id_User { get; set; } = default!;

        [ForeignKey(nameof(Id_User))]
        public AppUser User { get; set; } = default!;
        

        
        [ForeignKey(nameof(Manager))]
        public int? Id_Manager { get; set; }

        [ForeignKey(nameof(Id_Manager))]
        public AppUser? Manager { get; set; }



        [ForeignKey(nameof(Id_Position))]
        public int Id_Position { get; set; } = default!;
        
        [ForeignKey(nameof(Id_Position))]
        public Position Position { get; set; } = default!;




    }
}
