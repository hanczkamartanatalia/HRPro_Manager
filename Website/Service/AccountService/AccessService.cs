using Website.Entities;

namespace Website.Service.AccountService
{
    public static class AccessService
    {
        public static bool HasAccess(string _path, string? _roleName)
        {
            PermissionsGrid permission = EntityService<PermissionsGrid>.GetBy("Page", _path) ?? throw new Exception("There is no record in the database establishing access to the specified page: " + _path);
            if (permission.Id_Role == null) return true;

            Role role = EntityService<Role>.GetBy("Name", _roleName) ?? throw new Exception(_roleName + " don't exist. In database there is no this role name.");
            if (permission.Id_Role <= role.Id) return true;
            return false;
        }
    }
}
