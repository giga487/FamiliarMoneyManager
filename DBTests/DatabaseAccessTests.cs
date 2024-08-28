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
            test.CreateUsersTable("User");
            test.CreateGroupsTable("Groups");
        }


        [TestMethod]
        public void UserCreation()
        {
            MoneyAccessDB test = new MoneyAccessDB(null, "database.db");
            test.CreateUsersTable("User");
            test.CreateGroupsTable("Groups");

            List<string> names = new List<string>();

            int id = 0;
            for (int i = 0; i < 10; i++)
            {
                LoginAccount acc = new LoginAccount()
                {
                    Password = "",
                    UserName = $"GIGGINO{i}"
                };

                names.Add(acc.UserName);    
                test.AddUser(acc, ref id);
            }

            var account = test.GetAccount(id);

            Console.WriteLine($"Test GetAccount({id}) =>  {account.UserName}");

            var users = test.GetUsers();

            foreach (IAccount usersAcc in users)
            {
                Console.WriteLine($"{usersAcc.UserId} {usersAcc.UserName}");
            }
        }

        [TestMethod]
        public void GroupCreation()
        {
            MoneyAccessDB test = new MoneyAccessDB(null, "database.db");
            test.CreateUsersTable("User");
            test.CreateGroupsTable("Groups");

            List<int> userIds = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                int id = 0;
                LoginAccount acc = new LoginAccount()
                {
                    Password = "",
                    UserName = $"TEST{i}"
                };

                if (test.AddUser(acc, ref id) != DataDBAccess.DBResult.Success)
                {
                    Assert.Fail("Add user problem in group creation");
                }

                userIds.Add(id);
            }

            test.AddGroup("TEST_GROUP", userIds);

            var groupInfo = test.GetGroupInfo("TEST_GROUP");

            foreach (int id in groupInfo.UserIds)
            {
                var account = test.GetAccount(id);
                Console.WriteLine($"{groupInfo.GrouName}: {account.UserId} {account.UserName}");
            }
        }
    }
}