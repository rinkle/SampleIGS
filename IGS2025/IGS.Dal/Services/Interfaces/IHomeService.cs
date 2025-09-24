using IGS.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace IGS.Dal.Services.Interfaces
{
    public interface IHomeService
    {
        Task SaveHomeAsync(HomeViewModel model, IFormFile? brochure, string userId);
    }
}
