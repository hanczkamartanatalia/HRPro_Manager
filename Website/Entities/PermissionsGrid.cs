using System.ComponentModel.DataAnnotations.Schema;

namespace Website.Entities
{
    public class PermissionsGrid : Entity
    {
        public string Page { get; set; } = default!;

        [ForeignKey(nameof(Id_Role))]
        public Role Role { get; set; } = default!;

        [ForeignKey(nameof(Role))]
        public int Id_Role { get; set; } = default!;

        public bool HasAccess { get; set; } = false;
    }
}
