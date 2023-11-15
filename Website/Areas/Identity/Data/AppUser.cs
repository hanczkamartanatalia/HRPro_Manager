using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Website.Areas.Identity.Data;
public class AppUser : IdentityUser<int>
{
    public string Name { get; set; } = default!;
    public string LastName { get; set; } = default!;
}

