using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models;
using IGS.Models.KeyLessModels;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Implementations
{
    public class PortfolioServicesService : IPortfolioServicesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly GlobalEnvironmentSetting _globalEnvironment;
        private readonly ICommonListingService _commonListingService;

        public PortfolioServicesService(IUnitOfWork unitOfWork, ILoggerService logger, ICommonListingService commonListingService, GlobalEnvironmentSetting globalEnvironment)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _globalEnvironment = globalEnvironment;
            _commonListingService = commonListingService;
        }
        public async Task SavePortfolioServiceAsync(PortfolioServicesViewModel model)
        {
            try
            {
                if (model == null || model.PortfolioServices.Id == 0) return;

                var entity = await _unitOfWork.PortfolioService.GetAsync(h => h.Id == model.PortfolioServices.Id, tracked: true);
                if (entity == null) return;

                // Save related listings
                if (model.CoreAreasOfFocus?.Any() == true)
                {
                    await _commonListingService.SaveCommonListingAsync(model.CoreAreasOfFocus);
                }

                // Map fields
                entity.CoreAreasHeading = model.PortfolioServices.CoreAreasHeading;
                entity.CoreAreasDescription = model.PortfolioServices.CoreAreasDescription;
                entity.IndustryExpertiseHeading = model.PortfolioServices.IndustryExpertiseHeading;
                entity.IndustryExpertiseSubHeading = model.PortfolioServices.IndustryExpertiseSubHeading;
                entity.IndustryExpertiseDescription = model.PortfolioServices.IndustryExpertiseDescription;
                entity.FeaturedInsightHeading = model.PortfolioServices.FeaturedInsightHeading;
                entity.FeaturedInsightSubHeading = model.PortfolioServices.FeaturedInsightSubHeading;
                entity.FeaturedInsighDescription = model.PortfolioServices.FeaturedInsighDescription;
                entity.FeaturedInsighImage = model.PortfolioServices.FeaturedInsighImage;
                entity.FeaturedInsighPdf = model.PortfolioServices.FeaturedInsighPdf;
                entity.InsightHeading = model.PortfolioServices.InsightHeading;
                // Audit info
                entity.ModifiedDate = DateTime.Now;
                entity.ModifiedBy = _globalEnvironment.UserId;

                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in saving Portfolio Service");
                throw;
            }
        }
    }
}
