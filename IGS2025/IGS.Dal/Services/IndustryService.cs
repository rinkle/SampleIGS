using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Models;
using IGS.Models.KeyLessModels;
using System.Buffers.Text;
using System.Security.Claims;

namespace IGS.Dal.Services
{
    public interface IIndustryService
    {
        Task SaveIndustryAsync(GetIndustry_Result IndustryItem);
    }

    public class IndustryService : IIndustryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly GlobalEnvironmentSetting _globalEnvironment;

        public IndustryService(
            IUnitOfWork unitOfWork,
            ILoggerService logger,
            GlobalEnvironmentSetting globalEnvironment)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _globalEnvironment = globalEnvironment;
        }

        public async Task SaveIndustryAsync(GetIndustry_Result IndustryItem)
        {
            try
            {
                if (IndustryItem == null || IndustryItem.Id==0)
                    return; // nothing to process

                var industryData = await _unitOfWork.IndustryService
                       .GetAsync(h => h.Id == IndustryItem.Id, tracked: true);

                if (industryData != null)
                {
                    // Update fields
                    industryData.IndustryHeading = IndustryItem.IndustryHeading;
                    industryData.IndustryDescription = IndustryItem.IndustryDescription;
                    industryData.InsightHeading = IndustryItem.InsightHeading;
                    industryData.ModifiedDate = DateTime.Now;
                    industryData.ModifiedBy = _globalEnvironment.UserId;
                    await _unitOfWork.SaveAsync();
                }
                else
                {
                }


                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in Save Industry Async");
                throw; // rethrow if you want the controller to handle failure
            }
        }
    }
}
