using Website.Entities;
using Microsoft.AspNetCore.Http;

namespace Website.Service.AccountService
{
    public static class LoginService
    {
        public static LoginData Login(string login, string password)
        {
            LoginData loginData = EntityService<LoginData>.GetBy("Login", login);
            if (loginData == null) throw new Exception("Incorrect login.");
            PasswordService.isCorrect(password,loginData.Password);
            return loginData;
        }
        public static bool Logout()
        {
            throw new NotImplementedException();
        }

        private static void setUserSession(LoginData loginData)
        {
           // HttpContext http = new HttpContext();
           //.Session.SetString("UserName", "JohnDoe");
        }
    }
}
