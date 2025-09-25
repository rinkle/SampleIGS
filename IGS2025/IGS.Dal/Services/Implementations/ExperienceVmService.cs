using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Implementations
{
    public class ExperienceVmService : IExperienceVmService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;

        public ExperienceVmService(IUnitOfWork unitOfWork, ILoggerService logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ExperienceViewModel> GetExperienceVmAsync(
            string? industryCategoryIds = null,
            string? pageIds = null,
            string? orderBy = null,
            bool isAdmin = false)
        {
            try
            {
                // Experience SP with filters
                var experienceList = (await _unitOfWork.Experience
                    .GetExperienceFilterListFromSpAsync(industryCategoryIds, pageIds, orderBy)).ToList();

                // ✅ Always fetch categories (SP returns IEnumerable)
                var industryCategories = (await _unitOfWork.IndustryCategory
                    .GetIndustryCategoryFromSpAsync()).ToList();

                return new ExperienceViewModel(experienceList, industryCategories, isAdmin);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in ExperienceVmService.GetExperienceVmAsync");
                return new ExperienceViewModel(); // safe empty VM
            }
        }
    }
}
