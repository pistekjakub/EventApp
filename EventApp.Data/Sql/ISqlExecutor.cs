using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace EventApp.Data.Sql
{
    public interface ISqlExecutor
    {
        Task<int> ExecuteNonQueryStoredProcedureAsync(IDbConnection connection,
            string procedureName,
            object? @params = null,
            IDbTransaction? transaction = null);

        Task<int> ExecuteNonQueryAsync(IDbConnection connection,
            string sql,
            object? @params = null,
            IDbTransaction? transaction = null);

        Task<T> ExecuteScalarStoredProcedureAsync<T>(IDbConnection connection,
            string procedureName,
            object? @params = null,
            IDbTransaction? transaction = null);

        Task<T> ExecuteScalarAsync<T>(IDbConnection connection,
            string sql,
            object? @params = null,
            IDbTransaction? transaction = null);

        Task<T> ExecuteSingleRowResultStoredProcedureAsync<T>(IDbConnection connection,
            string procedureName,
            object? @params = null,
            IDbTransaction? transaction = null);

        Task<IEnumerable<T>> ExecuteQueryStoredProcedureAsync<T>(IDbConnection connection,
            string procedureName,
            object? @params = null,
            IDbTransaction? transaction = null);

        Task<IEnumerable<T>> ExecuteQueryAsync<T>(IDbConnection connection,
            string sql,
            object? @params = null,
            IDbTransaction? transaction = null);
    }
}
