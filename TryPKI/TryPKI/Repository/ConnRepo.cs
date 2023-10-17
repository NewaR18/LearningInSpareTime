using System.Data.SqlClient;
using System.Data;
using TryPKI.SqlConnector;

namespace TryPKI.Repository
{
    public class ConnRepo
    {
        public SqlConnection Connection()
        {
            string constr = new Connection().GetConnectionString();
            SqlConnection connection = new SqlConnection(constr);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            return connection;
        }
    }
}
