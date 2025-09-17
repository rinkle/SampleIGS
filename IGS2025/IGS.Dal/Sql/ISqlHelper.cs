using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IGS.Dal.Sql
{
    public interface ISqlHelper
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sqlOrProc, object? parameters = null, bool isStoredProc = false, CancellationToken ct = default);
        Task<T?> QuerySingleOrDefaultAsync<T>(string sqlOrProc, object? parameters = null, bool isStoredProc = false, CancellationToken ct = default);
        Task<int> ExecuteAsync(string sqlOrProc, object? parameters = null, bool isStoredProc = false, CancellationToken ct = default);
        Task<T?> ScalarAsync<T>(string sqlOrProc, object? parameters = null, bool isStoredProc = false, CancellationToken ct = default);
    }
}
