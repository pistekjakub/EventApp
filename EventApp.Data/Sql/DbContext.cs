using Microsoft.Extensions.Logging;
using System.Data;
using System.Threading.Tasks;

namespace EventApp.Data.Sql
{
    public class DbContext : IDbContext
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DbContext(ILogger<DbContext> logger, IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IDbConnection> GetConnection()
        {
            return await _connectionFactory.OpenConnection();
        }
    }
}
