using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Implementations
{
    public class IndustryVmService : IIndustryVmService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;


        public IndustryVmService(IUnitOfWork unitOfWork, ILoggerService logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IndustryViewModel> GetIndustryVmAsync(bool isAdmin = false)
        {
            try
            {
                var industryResult = await _unitOfWork.IndustryService.GetIndustryFromSpAsync();
                var allCategories = await _unitOfWork.IndustryService.GetIndustryCategoryFromSpAsync();
                return new IndustryViewModel(industryResult, allCategories, isAdmin);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in fetching Industry data");
                return null;
            }


        }
    }
}
