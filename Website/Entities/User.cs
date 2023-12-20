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
    [RegularExpression("^[A-Z][a-z]*$", ErrorMessage = "Invalid characters in the Name field. Use only letters and the first letter must be uppercase.")]
    public string Name { get; set; } = default!;
    [MaxLength(20)]
    [RegularExpression("^[A-Z][a-zA-Z-]+$", ErrorMessage = "Invalid characters in the LastName field. Use only letters or '-' character and first letter must be uppercase.")]
    public string LastName { get; set; } = default!;
    [MaxLength(20)]
    [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "The email address should end with @domain.com")]
    public string Email { get; set; } = default!;
}

