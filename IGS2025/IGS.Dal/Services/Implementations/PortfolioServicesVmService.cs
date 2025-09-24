using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Implementations
{
    public class PortfolioServicesVmService : IPortfolioServicesVmService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;

        public PortfolioServicesVmService(IUnitOfWork unitOfWork, ILoggerService logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<PortfolioServicesViewModel> GetPortfolioServicesVmAsync(bool isAdmin = false)
        {
            try
            {
                var portfolioServiceData = await _unitOfWork.PortfolioService.GetPortfolioServiceFromSpAsync();
                var allListings = await _unitOfWork.CommonListing.GetCommonListingFromSpAsync((int)PageEnum.PortfolioServices);

                return new PortfolioServicesViewModel(portfolioServiceData, allListings, isAdmin);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in fetching Portfolio Services data");
                return new PortfolioServicesViewModel(); // ✅ safe empty VM
            }
        }
    }
}
