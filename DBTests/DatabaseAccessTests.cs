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
        }

    }
}