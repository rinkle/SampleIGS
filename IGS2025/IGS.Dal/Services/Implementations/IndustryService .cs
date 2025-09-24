using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Services.Implementations
{
    public class IndustryService : IIndustryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly GlobalEnvironmentSetting _env;

        public IndustryService(IUnitOfWork unitOfWork, ILoggerService logger, GlobalEnvironmentSetting env)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _env = env;
        }

        public async Task SaveIndustryAsync(GetIndustry_Result IndustryItem)
        {
            try
            {
                if (IndustryItem == null || IndustryItem.Id == 0)
                    return;

                var industryData = await _unitOfWork.IndustryService
                    .GetAsync(h => h.Id == IndustryItem.Id, tracked: true);

                if (industryData != null)
                {
                    // Update fields
                    industryData.IndustryHeading = IndustryItem.IndustryHeading;
                    industryData.IndustryDescription = IndustryItem.IndustryDescription;
                    industryData.InsightHeading = IndustryItem.InsightHeading;
                    industryData.ModifiedDate = DateTime.Now;
                    industryData.ModifiedBy = _env.UserId;
                }

                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in SaveIndustryAsync");
                throw;
            }
        }
    }
}
