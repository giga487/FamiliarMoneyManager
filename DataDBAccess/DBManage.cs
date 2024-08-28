using System.Data.SQLite;

namespace DataDBAccess
{
    public enum DBResult
    {
        Success,
        ConnectionOK,
        ConnectionFail,
        GroupCreationOK,
        GroupCreationFail,
        CreateTableGroupOK,
        CreateTableGroupFail,
        CreateTableUsersOK,
        CreateTableUsersFail,

    }

    public class DBManage
    {
        protected Serilog.ILogger _logger { get; set; } = null;
        protected SQLiteConnection? _con;

        /// <summary>
        /// filename with type example database.db"
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="filename"></param>
        public DBManage(Serilog.ILogger logger, string filename)
        {

            _logger = logger;

            try
            {
                _con = new SQLiteConnection();
                string conn = @$"Data Source={filename}";//AttachDbFilename=|DataDirectory|\AccountService.mdf;Integrated Security=True";
                _con.ConnectionString = conn;
                _con.Open();
                logger?.Information($"Database connection: {DBResult.ConnectionOK}");
            }
            catch(Exception ex)
            {
                logger?.Error($"Database connection: {DBResult.ConnectionFail}");
                throw ex;
            }

        }
        private bool TableExist(string tableName)
        {
            string exist = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}';";

            using (SQLiteCommand cmd = new SQLiteCommand(exist, _con))
            {
                var t = cmd.ExecuteReader();
                if (t.HasRows)
                {
                    return true;
                }

                _logger?.Warning($"{tableName} not found");
                return false;
            }
        }
    }
}
