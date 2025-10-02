using IGS.Models;
using IGS.Models.ViewModels;
using System.Threading.Tasks;

namespace IGS.Dal.Services.Interfaces
{
    public interface INewsVmService
    {
        Task<NewsViewModel> GetNewsVmAsync(string? categoryIds = null, string? pageIds = null, string? orderBy = null, bool isAdmin = false);
        Task<NewsModel> GetNewsModelAsync(int newsId, string? categoryIds = null, string? pageIds = null, string? orderBy = null);
    }
}
