using IGS.Models.KeyLessModels;

namespace IGS.Dal.Services.Interfaces
{
    public interface IPortfolioServicesService
    {
        Task SavePortfolioServiceAsync(GetPortfolioService_Result portfolioService, string? userId);
    }
}
