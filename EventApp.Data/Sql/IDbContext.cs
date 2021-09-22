using System.Data;
using System.Threading.Tasks;

namespace EventApp.Data.Sql
{
    public interface IDbContext
    {
        Task<IDbConnection> GetConnection();
    }
}
