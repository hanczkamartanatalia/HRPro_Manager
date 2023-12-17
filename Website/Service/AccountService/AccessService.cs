using Website.Entities;
using Website.Migrations;

namespace Website.Service.AccountService
{
    public static class AccessService
    {
        public static bool HasAccess(string _path, string? _roleName)
        {
            string path = RemoveEntityIdFromPath(_path);

            PermissionsGrid permission = EntityService<PermissionsGrid>.GetBy("Page", path) ?? throw new Exception("There is no record in the database establishing access to the specified page: " + path);
            if (permission.Id_Role == null) return true;

            Role role = EntityService<Role>.GetBy("Name", _roleName) ?? throw new Exception(_roleName + " don't exist. In database there is no this role name.");
            if (permission.Id_Role <= role.Id) return true;
            return false;
        }

        private static string RemoveEntityIdFromPath(string input)
        {
            if(input.Length <= 1) return input;

            if (char.IsDigit(input[input.Length - 1]))
            {
                int lastSlashIndex = input.LastIndexOf('/');
                return input.Substring(0, lastSlashIndex);
            }
            return input;

        }
    }
}
