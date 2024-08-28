using System.Text.Json.Serialization;

namespace FamilyMoneyWebApp.src
{
    public class LoginAccount : IAccount
    { 
        public static string DefaultName = "Default";
        public string UserName { get; set; } = LoginAccount.DefaultName;
        public string Password { get; set; } = "";
        public DateTime CreationTime { get; set; } = DateTime.Now.ToUniversalTime();
        public int UserId { get; set; } = -1;
    }
    public interface IAccount
    {
        int UserId { get; }
        string UserName { get; }
        string Password { get; }
        DateTime CreationTime { get; }
    }

}
