using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Services
{
    public class CommonListingService : ICommonListingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly GlobalEnvironmentSetting _globalEnvironment;

        public CommonListingService(IUnitOfWork unitOfWork, ILoggerService logger, GlobalEnvironmentSetting globalEnvironment)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _globalEnvironment = globalEnvironment;
        }

        public async Task SaveCommonListingAsync(List<GetCommonListing_Result> commonListItems)
        {
            try
            {
                if (commonListItems == null || commonListItems.Count == 0)
                    return;

                foreach (var item in commonListItems)
                {
                    var commonListItem = await _unitOfWork.CommonListing.GetAsync(
                        c => c.Id == item.Id,
                        tracked: true);

                    if (commonListItem == null)
                    {
                        commonListItem = new CommonListing
                        {
                            CreatedBy = _globalEnvironment.UserId,
                            CreatedDate = DateTime.Now,
                            IsActive = true
                        };
                        await _unitOfWork.CommonListing.AddAsync(commonListItem);
                    }

                    // map fields
                    commonListItem.Fk_PageId = item.Fk_PageId;
                    commonListItem.Section = item.Section;
                    commonListItem.Heading = item.Heading;
                    commonListItem.SubHeading = item.SubHeading;
                    commonListItem.Description = item.Description;
                    commonListItem.UploadedData = item.UploadedData;
                    commonListItem.AdditionalImage1 = item.AdditionalImage1;
                    commonListItem.AdditionalImage2 = item.AdditionalImage2;
                    commonListItem.AdditionalImage3 = item.AdditionalImage3;
                    commonListItem.ImageLabel = item.ImageLabel;
                    commonListItem.AdditionalSubHeading = item.AdditionalSubHeading;
                    commonListItem.ReferanceUrl = item.ReferanceUrl;
                    commonListItem.DisplayOrder = item.DisplayOrder;

                    if (item.Fk_CaseStudyId.HasValue)
                        commonListItem.Fk_CaseStudyId = item.Fk_CaseStudyId.Value;

                    if (item.Fk_PortId.HasValue)
                        commonListItem.Fk_PortId = item.Fk_PortId.Value;

                    // audit fields
                    commonListItem.ModifiedBy = _globalEnvironment.UserId;
                    commonListItem.ModifiedDate = DateTime.Now;
                }

                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in SaveCommonListingAsync");
            }
        }
    }
}
