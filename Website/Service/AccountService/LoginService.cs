using Website.Entities;

namespace Website.Service.AccountService
{
    public static class LoginService
    {
        public static bool Login(string login, string password)
        {
            try
            {
                LoginData loginData = EntityService<LoginData>.GetBy("Login", login);
                

            }
            catch
            {
                return false;
            }

            return true;            
        }
        public static bool Logout()
        {
            throw new NotImplementedException();
        }
    }
}
