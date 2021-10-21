using System.Data.Common;
using System.Data.SqlClient;

namespace AcmeCaseAPI.Persistence
{
    public class DbConnect : IDbConnect
    {
        private readonly string _connectionString;

        public DbConnect(string connectionString)
        {
            _connectionString = connectionString;
        }
        public DbConnection Create()
        {
            return new SqlConnection(_connectionString);
        }

    }
}
