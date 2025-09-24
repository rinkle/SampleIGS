using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Implementations
{
    public class HomeVmService : IHomeVmService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeVmService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<HomeViewModel> GetHomeVmAsync(bool isAdmin = false)
        {
            var homeResult = await _unitOfWork.Home.GetHomeFromSpAsync();
            var allListings = await _unitOfWork.CommonListing.GetCommonListingFromSpAsync((int)PageEnum.Home);

            return new HomeViewModel(homeResult, allListings, isAdmin);
        }
    }
}
