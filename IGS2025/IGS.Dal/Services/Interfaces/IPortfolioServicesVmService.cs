using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Interfaces
{
    public interface IPortfolioServicesVmService
    {
        Task<PortfolioServicesViewModel> GetPortfolioServicesVmAsync(bool isAdmin = false);
    }
}
