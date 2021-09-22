using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace EventApp.Data.Sql
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        public async Task<IDbConnection> OpenConnection()
        {
            // TODO read it from configuration
            // SqlConnection connection = new SqlConnection(Startup.ValoDispatchDbConnectionString);
            SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=EventApp;Trusted_Connection=True;");           
            try
            {
                await connection.OpenAsync();
            }
            catch (Exception)
            {
                connection.Dispose();
                throw;
            }
            return connection;
        }
    }
}
