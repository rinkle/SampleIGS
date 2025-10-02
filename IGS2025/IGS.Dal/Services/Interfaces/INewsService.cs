using IGS.Models;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Interfaces
{
    public interface INewsService
    {
        Task<int> SaveNewsAsync(NewsModel model);
        Task<bool> DeleteNewsAsync(int id);
        Task<bool> DeleteLogoAsync(int id, Action<News> clearLogoAction);
    }
}
