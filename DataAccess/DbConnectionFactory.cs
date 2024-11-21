using System.Data;
using System.Data.SqlClient;

namespace CarModelManagementAPI.DataAccess
{
    public class DbConnectionFactory: IDbConnectionFactory
    {
        private readonly string _connectionString;
        public DbConnectionFactory(string connectionString)
        {
                this._connectionString = connectionString;
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
