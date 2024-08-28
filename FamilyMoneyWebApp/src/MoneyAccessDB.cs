
using DataDBAccess;
using System.Data.SQLite;

namespace FamilyMoneyWebApp.src
{
    public class MoneyAccessDB : DataDBAccess.DBManage
    {
        public int MaxUserGroupNumber { get; private set; } = 15;
        private string _groupNameTable { get; set; } = "";
        private string _usersNameTable { get; set; } = "";
        public MoneyAccessDB(Serilog.ILogger logger, string filename) : base(logger, filename)
        {

        }
        public bool CreateGroupsTable(string groupNameTable)
        {
            string groupNameString = string.Empty;
            _groupNameTable = groupNameTable;

            for (int i = 0; i < MaxUserGroupNumber; i++)
            {
                groupNameString += $", user_id{i} int";
            }

            string sql = $"Create Table [{_groupNameTable}] (name varchar(40){groupNameString})";

            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, _con))
                {
                    cmd.ExecuteNonQuery();
                }

                _logger?.Information($"Group creation: {DBResult.GroupCreationOK}");
            }
            catch
            {
                _logger?.Information($"Group creation: {DBResult.GroupCreationFail}");
            }

            return true;
        }
        public bool CreateUsersTable(string tableName)
        {
            _usersNameTable = tableName;
            string sql = $"Create Table [{_usersNameTable}] (userid int, name varchar(40), creation DATETIME)";

            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, _con))
                {
                    cmd.ExecuteNonQuery();
                }

                _logger?.Information($"Group creation: {DBResult.GroupCreationOK}");
            }
            catch
            {
                _logger?.Information($"Group creation: {DBResult.GroupCreationFail}");
            }

            return true;
        }
    }
}
