using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using Dapper;

namespace EventApp.Data.Sql
{
    public class SqlExecutor : ISqlExecutor
    {
        public async Task<int> ExecuteNonQueryStoredProcedureAsync(IDbConnection connection, string procedureName, object? @params = null, IDbTransaction? transaction = null)
        {
            return await connection.ExecuteAsync(procedureName, @params, commandType: CommandType.StoredProcedure, transaction: transaction);
        }

        public async Task<int> ExecuteNonQueryAsync(IDbConnection connection, string sql, object? @params = null, IDbTransaction? transaction = null)
        {
            return await connection.ExecuteAsync(sql, @params, commandType: CommandType.Text, transaction: transaction);
        }

        public async Task<IEnumerable<T>> ExecuteQueryStoredProcedureAsync<T>(IDbConnection connection, string procedureName, object? @params = null, IDbTransaction? transaction = null)
        {
            return await connection.QueryAsync<T>(procedureName, @params, commandType: CommandType.StoredProcedure, transaction: transaction);
        }

        public async Task<T> ExecuteScalarStoredProcedureAsync<T>(IDbConnection connection, string procedureName, object? @params = null, IDbTransaction? transaction = null)
        {
            return await connection.ExecuteScalarAsync<T>(procedureName, @params, commandType: CommandType.StoredProcedure, transaction: transaction);
        }

        public async Task<T> ExecuteScalarAsync<T>(IDbConnection connection, string sql, object? @params = null, IDbTransaction? transaction = null)
        {
            return await connection.ExecuteScalarAsync<T>(sql, @params, transaction: transaction);
        }
        public async Task<T> ExecuteSingleRowResultStoredProcedureAsync<T>(IDbConnection connection, string procedureName, object? @params = null, IDbTransaction? transaction = null)
        {
            return await connection.QueryFirstOrDefaultAsync<T>(procedureName, @params, commandType: CommandType.StoredProcedure, transaction: transaction);
        }

        public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(IDbConnection connection, string sql, object? @params = null, IDbTransaction? transaction = null)
        {
            return await connection.QueryAsync<T>(sql, @params, transaction);
        }
    }
}
