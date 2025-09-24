using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;
using IGS.ViewModels;

namespace IGS.Dal.Services.Implementations
{
    public class IndustryVmService : IIndustryVmService
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndustryVmService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IndustryViewModel> GetIndustryVmAsync(bool isAdmin = false)
        {
            var industryResult = await _unitOfWork.IndustryService.GetIndustryFromSpAsync();
            var allCategories = await _unitOfWork.IndustryService.GetIndustryCategoryFromSpAsync();

            return new IndustryViewModel(industryResult, allCategories, isAdmin);
        }
    }
}
