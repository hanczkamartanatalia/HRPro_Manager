using Website.Entities;

namespace Website.Models.Account
{
    public class Register
    {
        public User User { get; set; } = default!;
        public LoginData LoginData { get; set; } = default!;
    }
}
