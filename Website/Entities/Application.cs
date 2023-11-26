using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.Entities
{
    public class Application: Entity
    {
         [ForeignKey(nameof(User))]
        public int Id_User { get; set; } = default!;

        [ForeignKey(nameof(Id_User))]
        public User User { get; set; } = default!;

        public DateTime StartDate { get; set; } = default!;
        public DateTime EndDate { get; set; } = default!;
    }
}
