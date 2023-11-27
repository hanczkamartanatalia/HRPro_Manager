using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Website.Entities;
public class User : Entity
{
    [MaxLength(20)]
    public string Namee { get; set; } = default!;
    [MaxLength(20)]
    public string LastName { get; set; } = default!;
    [MaxLength(20)]
    public string Email { get; set; } = default!;
}

