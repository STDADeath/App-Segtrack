using MySql.Data.MySqlClient;

namespace segtrack.Services
{
    public class DatabaseService
    {
        private const string SERVER = "192.168.56.1";
        private const string DATABASE = "dbsegtrack";
        private const string USER = "root";
        private const string PASSWORD = "";
        private const string PORT = "3306";

        private string GetConnectionString()
        {
            return $"Server={SERVER};Port={PORT};Database={DATABASE};Uid={USER};Pwd={PASSWORD};SslMode=None;";
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var connection = new MySqlConnection(GetConnectionString());
                await connection.OpenAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
