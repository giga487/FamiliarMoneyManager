using FamilyMoneyWebApp.src;

namespace DBTests
{
    [TestClass]
    public class DatabaseAccessTests
    {
        [TestMethod]
        public void AccessDBUT()
        {
            MoneyAccessDB test = new MoneyAccessDB(null, "database.db");
        }

        [TestMethod]
        public void GroupCreation()
        {
            MoneyAccessDB test = new MoneyAccessDB(null, "database.db");

            test.CreateGroupsTable("Groups");
        }


        [TestMethod]
        public void UserCreation()
        {
            MoneyAccessDB test = new MoneyAccessDB(null, "database.db");

            test.CreateUsersTable("User");
            List<string> names = new List<string>();

            for (int i = 0; i < 10; i++)
            {
                LoginAccount acc = new LoginAccount()
                {
                    Password = "",
                    UserName = $"GIGGINO{i}"
                };

                names.Add(acc.UserName);    
                test.AddUser(acc);
            }

            var users = test.GetUsers();

            foreach (IAccount usersAcc in users)
            {
                Console.WriteLine($"{usersAcc.UserId} {usersAcc.UserName}");
            }
        }

    }
}