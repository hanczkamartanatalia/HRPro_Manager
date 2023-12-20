using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.Entities
{
    public class Employment: Entity
    {
        public DateTime EmploymentDate { get; set; } = default!;
        
        [RegularExpression(@"^(\d{1,2}(\.\d{1,2})?|100)$", ErrorMessage = "Rate must be a valid decimal up to 100")]
        public decimal Rate { get; set; } = default!;


        [ForeignKey(nameof(User))]
        public int Id_User { get; set; } = default!;

        [ForeignKey(nameof(Id_User))]
        public User User { get; set; } = default!;

        [ForeignKey(nameof(Manager))]
        public int? Id_Manager { get; set; }

        [ForeignKey(nameof(Id_Manager))]
        public User? Manager { get; set; }

        [ForeignKey(nameof(Id_Position))]
        public int Id_Position { get; set; } = default!;
        
        [ForeignKey(nameof(Id_Position))]
        public Position Position { get; set; } = default!;
    }
}
