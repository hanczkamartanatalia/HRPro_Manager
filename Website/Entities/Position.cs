using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Website.Entities
{
    public class Position: Entity
    {
        [MaxLength(40)]
        public string Name { get; set; } = default!;
    }
}
