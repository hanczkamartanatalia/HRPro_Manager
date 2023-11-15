using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.Areas.Identity.Data;

namespace Database.Entities
{
    public class LoginData : IdentityUserLogin<int>
    {
        [ForeignKey(nameof(User))]
        public int Id_User { get; set; } = default!; 
        
        [ForeignKey(nameof(Id_User))] 
        public AppUser User { get; set; } = default!;


        [ForeignKey(nameof(Role))]
        public int Id_Role { get; set; } = default!;
        
        [ForeignKey(nameof(Id_Role))]
        public Role Role { get; set; } = default!;
    }
}
