using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGS.Dal.Services.Interfaces
{
    public interface ILoggerService
    {
        Task<int> LogErrorAsync(Exception e, string? details = null);
    }
}
