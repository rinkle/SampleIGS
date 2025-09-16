using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace IGS.Dal.SqlHelper
{
    public class SqlHelper : ISqlHelper
    {
        private readonly string _connString;

        public SqlHelper(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("DefaultConnection")
                           ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is missing.");
        }

        private async Task<IDbConnection> CreateOpenConnectionAsync(CancellationToken ct)
        {
            var conn = new SqlConnection(_connString);
            await conn.OpenAsync(ct);
            return conn;
        }

        /// <summary>
        /// Converts SqlParameter[] to Dapper DynamicParameters if necessary.
        /// </summary>
        private static object? NormalizeParameters(object? parameters)
        {
            if (parameters is SqlParameter[] sqlParams)
            {
                var dyn = new DynamicParameters();
                foreach (var p in sqlParams)
                {
                    dyn.Add(
                        name: p.ParameterName.TrimStart('@'), // remove '@' for Dapper mapping
                        value: p.Value,
                        dbType: p.DbType,
                        direction: p.Direction,
                        size: p.Size == 0 ? null : p.Size
                    );
                }
                return dyn;
            }

            return parameters; // already fine for anonymous objects / DynamicParameters
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sqlOrProc, object? parameters = null, bool isStoredProc = false, CancellationToken ct = default)
        {
            using var conn = await CreateOpenConnectionAsync(ct);
            var cmd = new CommandDefinition(sqlOrProc, NormalizeParameters(parameters),
                                            commandType: isStoredProc ? CommandType.StoredProcedure : CommandType.Text,
                                            cancellationToken: ct);
            return await conn.QueryAsync<T>(cmd);
        }

        public async Task<T?> QuerySingleOrDefaultAsync<T>(string sqlOrProc, object? parameters = null, bool isStoredProc = false, CancellationToken ct = default)
        {
            using var conn = await CreateOpenConnectionAsync(ct);
            var cmd = new CommandDefinition(sqlOrProc, NormalizeParameters(parameters),
                                            commandType: isStoredProc ? CommandType.StoredProcedure : CommandType.Text,
                                            cancellationToken: ct);
            return await conn.QuerySingleOrDefaultAsync<T>(cmd);
        }

        public async Task<int> ExecuteAsync(string sqlOrProc, object? parameters = null, bool isStoredProc = false, CancellationToken ct = default)
        {
            using var conn = await CreateOpenConnectionAsync(ct);
            var cmd = new CommandDefinition(sqlOrProc, NormalizeParameters(parameters),
                                            commandType: isStoredProc ? CommandType.StoredProcedure : CommandType.Text,
                                            cancellationToken: ct);
            return await conn.ExecuteAsync(cmd);
        }

        public async Task<T?> ScalarAsync<T>(string sqlOrProc, object? parameters = null, bool isStoredProc = false, CancellationToken ct = default)
        {
            using var conn = await CreateOpenConnectionAsync(ct);
            var cmd = new CommandDefinition(sqlOrProc, NormalizeParameters(parameters),
                                            commandType: isStoredProc ? CommandType.StoredProcedure : CommandType.Text,
                                            cancellationToken: ct);
            return await conn.ExecuteScalarAsync<T>(cmd);
        }
    }
}
