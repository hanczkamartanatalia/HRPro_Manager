﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website.Entities
{
    public class PermissionsGrid : Entity
    {
        [MaxLength(50)]
        public string Page { get; set; } = default!;

        [ForeignKey(nameof(Id_Role))]
        public Role? Role { get; set; } = null;

        [ForeignKey(nameof(Role))]
        public int? Id_Role { get; set; } = null;

    }
}
