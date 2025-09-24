using IGS.Models.KeyLessModels;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Interfaces
{
    public interface IPortfolioServicesService
    {
        Task SavePortfolioServiceAsync(PortfolioServicesViewModel portfolioService);
    }
}
