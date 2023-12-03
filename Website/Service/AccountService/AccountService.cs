using Website.Entities;

namespace Website.Service.AccountService
{
    public static class AccountService
    {
        public static bool HasAccess(int? id, List<string>? expectedRoleNames = null)
        {
            if (id == null) { return false; }

            LoginData loginData = EntityService<LoginData>.GetById((int)id);
            if(loginData == null) { return false; }

            if (expectedRoleNames == null) { return true; }
            Role role = EntityService<Role>.GetById(loginData.Id_Role);

            string? roleName = expectedRoleNames.FirstOrDefault(x => x == role.Name);
            if (roleName == null) { return false; }
            return true;
        }
    }
}
