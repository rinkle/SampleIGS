using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Services.Implementations
{
    public class IndustryCategoryService : IIndustryCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly GlobalEnvironmentSetting _globalEnvironment;

        public IndustryCategoryService(
            IUnitOfWork unitOfWork,
            ILoggerService logger,
            GlobalEnvironmentSetting globalEnvironment)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _globalEnvironment = globalEnvironment;
        }

        public async Task SaveIndustryCategoryAsync(List<GetIndustryCategory_Result> industryCategoryListItems)
        {
            try
            {
                if (industryCategoryListItems == null || industryCategoryListItems.Count == 0)
                    return; // nothing to process

                foreach (var item in industryCategoryListItems)
                {
                    var category = await _unitOfWork.IndustryCategory.GetAsync(
                        c => c.Id == item.Id,
                        tracked: true);

                    if (category == null)
                    {
                        // Insert new only if Id == 0 AND DisplayOrder provided
                        if (item.Id == 0 && item.DisplayOrder.HasValue)
                        {
                            category = new IndustryCategory
                            {
                                CreatedBy = _globalEnvironment.UserId,
                                CreatedDate = DateTime.Now,
                                IsActive = true
                            };
                            await _unitOfWork.IndustryCategory.AddAsync(category);
                        }
                        else
                        {
                            continue; // skip invalid record
                        }
                    }

                    // Map fields (both insert/update)
                    category.IndustryName = item.IndustryName;
                    category.IndustryDescription = item.IndustryDescription;
                    category.DisplayOrder = item.DisplayOrder;

                    // Audit
                    category.ModifiedBy = _globalEnvironment.UserId;
                    category.ModifiedDate = DateTime.Now;
                }

                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in SaveIndustryCategoryAsync");
                throw; // rethrow if you want the controller to handle failure
            }
        }
    }
}
