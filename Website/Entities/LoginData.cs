﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.Service.AccountService;

namespace Website.Entities
{
    public class LoginData : Entity
    {
        public LoginData(string login, string password, int id_User, User user, int id_Role, Role role)
        {
            Login = login;
            Password = PasswordService.HashPassword(password);
            Id_User = id_User;
            User = user;
            Id_Role = id_Role;
            Role = role;
        }
        public LoginData(){ }

        [MaxLength(20)]
        [RegularExpression(@"^(?=(?:.*[a-zA-Z]){3})[a-zA-Z0-9._-]+$", ErrorMessage = "Login must contain at least three letters and can only include letters, numbers and this symbol: . _ -")]
        public string Login { get; set; } = default!;

        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+.\\-])[A-Za-z\d!@#$%^&*()_+.\\-]{8,}$", ErrorMessage = "The password should be at least 8 characters long, contain at least one uppercase letter, one number and one special character.")]
        public string Password { get; set; } = default!;

        [ForeignKey(nameof(User))]
        public int Id_User { get; set; } = default!;

        [ForeignKey(nameof(Id_User))]
        public User User { get; set; } = default!;


        [ForeignKey(nameof(Role))]
        public int Id_Role { get; set; } = default!;

        [ForeignKey(nameof(Id_Role))]
        public Role Role { get; set; } = default!;
    }
}
