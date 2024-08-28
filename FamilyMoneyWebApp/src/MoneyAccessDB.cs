
using DataDBAccess;
using System.Data.SQLite;
using System.Reflection.PortableExecutable;
using System.Security.Principal;

namespace FamilyMoneyWebApp.src
{
    public class MoneyAccessDB : DataDBAccess.DBManage
    {
        public int MaxUserGroupNumber { get; private set; } = 15;
        private string _groupNameTable { get; set; } = "";
        private string _usersNameTable { get; set; } = "";

        public string _userIdField { get; set; } = "userid";
        public MoneyAccessDB(Serilog.ILogger logger, string filename) : base(logger, filename)
        {

        }
        public DBResult CreateGroupsTable(string groupNameTable)
        {
            string groupNameString = string.Empty;
            _groupNameTable = groupNameTable;

            if (TableExist(groupNameTable))
            {
                return DBResult.CreateTableAlreadyExixt;
            }

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

                _logger?.Information($"Create GROUP Table {groupNameTable}: {DBResult.GroupCreationOK}");
            }
            catch
            {
                _logger?.Information($"Create GROUP Table {groupNameTable}: {DBResult.GroupCreationFail}");
                return DBResult.CreateTableGroupFail;
            }

            return DBResult.Success;
        }
        public DBResult CreateUsersTable(string tableName)
        {
            _usersNameTable = tableName;

            if (TableExist(tableName))
            {
                return DBResult.CreateTableAlreadyExixt;
            }

            string sql = $"Create Table [{_usersNameTable}] ({_userIdField} int, name varchar(40), password varchar(40), creation DATETIME)";

            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, _con))
                {
                    cmd.ExecuteNonQuery();
                }

                _logger?.Information($"Create Table User: {DBResult.CreateTableUsersOK}");

            }
            catch
            {
                _logger?.Information($"Create Table User: {DBResult.CreateTableUsersFail}");
                return DBResult.CreateTableUsersFail;
            }

            return DBResult.Success;
        }


        public void AddGroup(string groupName, List<int> userIds)
        {
            string groupNameString = string.Empty;
            string groupNameStringValue = string.Empty;

            for (int i = 0; i < MaxUserGroupNumber; i++)
            {
                groupNameString += $", user_id{i}";
                groupNameStringValue += $", @user_id{i}";
            }

            _logger?.Information($"Group Creation {groupName}");
            string cmdCreateSecret = $"INSERT INTO {_groupNameTable} (name {groupNameString}) VALUES (@name {groupNameStringValue})";

            int userId = GetNewUserId();

            using (SQLiteCommand command = new SQLiteCommand(cmdCreateSecret, _con))
            {
                command.Parameters.AddWithValue("@name", groupName);

                for(int i = 0; i < MaxUserGroupNumber; i++)// (int id in userIds)
                {
                    try
                    {
                        command.Parameters.AddWithValue($"@user_id{i}", userIds[i]);
                    }
                    catch
                    {
                        command.Parameters.AddWithValue($"@user_id{i}", string.Empty);
                    }
                }

                int result = command.ExecuteNonQuery();
            }
        }

        public DBResult AddUser(IAccount account, ref int userId)
        {
            if (account is null || !(account is LoginAccount acc))
                return DBResult.CreateUserFail;

            _logger?.Information($"Add {account.UserName} user");
            string cmdCreateSecret = $"INSERT INTO {_usersNameTable} ({_userIdField}, name, password, creation) VALUES (@{_userIdField}, @name, @password, @datetime)";

            try
            {
                userId = GetNewUserId();

                using (SQLiteCommand command = new SQLiteCommand(cmdCreateSecret, _con))
                {
                    command.Parameters.AddWithValue($"@{_userIdField}", userId);
                    command.Parameters.AddWithValue("@name", acc.UserName);
                    command.Parameters.AddWithValue("@password", acc.Password);
                    command.Parameters.AddWithValue("@datetime", acc.CreationTime);
                    int result = command.ExecuteNonQuery();
                }

                return DBResult.Success;
            }
            catch
            {
                return DBResult.CreateUserFail;
            }
        }



        public List<IAccount> GetUsers()
        {
            string cmdCreateSecret = $"SELECT * FROM {_usersNameTable}";
            List<IAccount> userList = new List<IAccount>();

            using (SQLiteCommand command = new SQLiteCommand(cmdCreateSecret, _con))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            LoginAccount account = new LoginAccount();
                            if (reader[0] is int userID)
                            {
                                account.UserId= (int)userID;
                            }
                            if (reader[1] is string name)
                            {
                                account.UserName = name;
                            }

                            if (reader[2] is string pwd)
                            {
                                account.Password = pwd;
                            }

                            if (reader[3] is DateTime date)
                            {
                                account.CreationTime = date;
                            }

                            userList.Add(account);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                }
            }

            return userList;
        }

        public int GetNewUserId()
        {
            using (SQLiteCommand command = new SQLiteCommand($"SELECT MAX({_userIdField}) FROM {_usersNameTable}", _con))
            {
                try
                {
                    return Convert.ToInt32(command.ExecuteScalar()) + 1;
                }
                catch
                {
                    return 0;
                }         
            }
        }
    }
}
