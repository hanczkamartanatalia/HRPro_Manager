﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Website.Entities;
public class User : Entity
{
    [MaxLength(20)]
    [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Invalid characters in the Name field. Use only letters.")]
    public string Name { get; set; } = default!;
    [MaxLength(20)]
    [RegularExpression("^[a-zA-Z-]+$", ErrorMessage = "Invalid characters in the LastName field. Use only letters lub '-' character.")]
    public string LastName { get; set; } = default!;
    [MaxLength(20)]
    [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "Invalid Email.")]
    public string Email { get; set; } = default!;
}

