using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Services.Implementations
{
    public class PortfolioServicesService : IPortfolioServicesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly GlobalEnvironmentSetting _globalEnvironment;

        public PortfolioServicesService(IUnitOfWork unitOfWork, ILoggerService logger, GlobalEnvironmentSetting globalEnvironment)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _globalEnvironment = globalEnvironment;
        }

        public async Task SavePortfolioServiceAsync(GetPortfolioService_Result portfolioService, string? userId)
        {
            try
            {
                if (portfolioService == null || portfolioService.Id == 0) return;

                var entity = await _unitOfWork.PortfolioService.GetAsync(h => h.Id == portfolioService.Id, tracked: true);
                if (entity == null) return;

                // Map fields
                entity.CoreAreasHeading = portfolioService.CoreAreasHeading;
                entity.CoreAreasDescription = portfolioService.CoreAreasDescription;
                entity.IndustryExpertiseHeading = portfolioService.IndustryExpertiseHeading;
                entity.IndustryExpertiseSubHeading = portfolioService.IndustryExpertiseSubHeading;
                entity.IndustryExpertiseDescription = portfolioService.IndustryExpertiseDescription;
                entity.FeaturedInsightHeading = portfolioService.FeaturedInsightHeading;
                entity.FeaturedInsightSubHeading = portfolioService.FeaturedInsightSubHeading;
                entity.FeaturedInsighDescription = portfolioService.FeaturedInsighDescription;
                entity.FeaturedInsighImage = portfolioService.FeaturedInsighImage;
                entity.FeaturedInsighPdf = portfolioService.FeaturedInsighPdf;
                entity.InsightHeading = portfolioService.InsightHeading;

                // Audit info
                entity.ModifiedDate = DateTime.Now;
                entity.ModifiedBy = userId ?? _globalEnvironment.UserId;

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
