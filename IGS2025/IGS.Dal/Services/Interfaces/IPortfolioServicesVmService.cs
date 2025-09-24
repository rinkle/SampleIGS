using IGS.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace IGS.Dal.Services.Interfaces
{
    public interface IPortfolioServicesVmService
    {
        Task<PortfolioServicesViewModel> GetPortfolioServicesVmAsync(bool isAdmin = false);
        Task SavePortfolioServicesAsync(PortfolioServicesViewModel model, IFormFile? brochure, string? userId);
    }
}
