using Website.Entities;

namespace Website.Service.AccountService
{
    public static class AccountService
    {
        public static bool HasAccess(List<string> expectedRoleNames,LoginData loginData)
        {
            Role role = EntityService<Role>.GetById(loginData.Id_Role);
            string roleName = expectedRoleNames.FirstOrDefault(x => x == role.Name);
            if (roleName == null) { return false; }
            return true;
        }

    }
}
