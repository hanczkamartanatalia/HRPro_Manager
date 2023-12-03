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

        public static bool isLogin(int? id)
        {
            if(id == null) return false;
            else return true;
        }
    }
}
