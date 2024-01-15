using Website.Entities;

namespace Website.Service.AccountService
{
    public static class AccountService
    {
        public static Dictionary<string,string>? AccountWall(LoginData input)
        {
            Dictionary<string,string> output = new();
            //Application? application = EntityService<Application>.GetLastRecord("Id_User", (input.Id).ToString());
            Application? application = EntityService<Application>.GetLastRecord();

            if (application == null) return null;

            Category? category = EntityService<Category>.GetById(application.Id_Category);
            output.Add("Start", (application.StartDate).ToShortDateString());
            output.Add("End", (application.EndDate).ToShortDateString());
            output.Add("Category", category.Name);
            return output;
        }
    }
}
