using Website.Entities;

namespace Website.Service.AccountService
{
    public static class AccountService
    {
        public static bool HasAccess(string expectedRoleName,LoginData loginData)
        {
            Role role = EntityService<Role>.GetById(loginData.Id_Role);
            if(expectedRoleName == role.Name) return true;
            return false;
        }

    }
}
