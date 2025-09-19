using IGS.Dal.Data;
using IGS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGS.Dal.Services
{
    public interface ILoggerService
    {
        Task<int> LogErrorAsync(Exception e, string? details = null);
    }
    public class LoggerService : ILoggerService
    {
        private readonly ApplicationDbContext _db;

        public LoggerService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<int> LogErrorAsync(Exception e, string? details = null)
        {
            try
            {
                var log = new ErrorLog
                {
                    Message = e.Message,
                    InnerMessage = e.InnerException?.Message ?? string.Empty,
                    Source = e.Source ?? string.Empty,
                    StackTrace = e.StackTrace,
                    ErrorDate = DateTime.UtcNow,
                    Details = details
                };

                _db.ErrorLogs.Add(log);
                await _db.SaveChangesAsync();

                return log.Id;
            }
            catch
            {
                return 0; // swallow to avoid recursive logging failures
            }
        }
    }
}