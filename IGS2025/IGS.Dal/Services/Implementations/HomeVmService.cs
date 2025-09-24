using Azure.Core;
using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Implementations
{
    public class HomeVmService : IHomeVmService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;

        public HomeVmService(IUnitOfWork unitOfWork, ILoggerService logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<HomeViewModel> GetHomeVmAsync(bool isAdmin = false)
        {
            try
            {
                var homeResult = await _unitOfWork.Home.GetHomeFromSpAsync();
                var allListings = await _unitOfWork.CommonListing.GetCommonListingFromSpAsync((int)PageEnum.Home);
                return new HomeViewModel(homeResult, allListings, isAdmin);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in fetching data");
                return null;
            }
          
        }
    }
}
