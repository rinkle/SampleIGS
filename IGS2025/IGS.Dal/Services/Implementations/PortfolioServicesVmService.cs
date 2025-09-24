using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace IGS.Dal.Services.Implementations
{
    public class PortfolioServicesVmService : IPortfolioServicesVmService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly ICommonListingService _commonListingService;

        public PortfolioServicesVmService(
            IUnitOfWork unitOfWork,
            ILoggerService logger,
            ICommonListingService commonListingService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _commonListingService = commonListingService;
        }

        public async Task<PortfolioServicesViewModel> GetPortfolioServicesVmAsync(bool isAdmin = false)
        {
            var portfolioServiceData = await _unitOfWork.PortfolioService.GetPortfolioServiceFromSpAsync();
            var allListings = await _unitOfWork.CommonListing.GetCommonListingFromSpAsync((int)PageEnum.PortfolioServices);

            return new PortfolioServicesViewModel(portfolioServiceData, allListings, isAdmin);
        }

        public async Task SavePortfolioServicesAsync(PortfolioServicesViewModel model, IFormFile? brochure, string? userId)
        {
            if (model.PortfolioServices == null) return;

            // ✅ Save related listings using the service
            if (model.CoreAreasOfFocus?.Any() == true)
            {
                await _commonListingService.SaveCommonListingAsync(model.CoreAreasOfFocus);
            }

            // Fetch existing record
            var portfolioServiceData = await _unitOfWork.PortfolioService.GetAsync(h => h.Id == model.PortfolioServices.Id, tracked: true);
            if (portfolioServiceData == null) return;

            // Map fields
            portfolioServiceData.CoreAreasHeading = model.PortfolioServices.CoreAreasHeading;
            portfolioServiceData.CoreAreasDescription = model.PortfolioServices.CoreAreasDescription;
            portfolioServiceData.IndustryExpertiseHeading = model.PortfolioServices.IndustryExpertiseHeading;
            portfolioServiceData.IndustryExpertiseSubHeading = model.PortfolioServices.IndustryExpertiseSubHeading;
            portfolioServiceData.IndustryExpertiseDescription = model.PortfolioServices.IndustryExpertiseDescription;
            portfolioServiceData.FeaturedInsightHeading = model.PortfolioServices.FeaturedInsightHeading;
            portfolioServiceData.FeaturedInsightSubHeading = model.PortfolioServices.FeaturedInsightSubHeading;
            portfolioServiceData.FeaturedInsighDescription = model.PortfolioServices.FeaturedInsighDescription;
            portfolioServiceData.FeaturedInsighImage = model.PortfolioServices.FeaturedInsighImage;
            portfolioServiceData.FeaturedInsighPdf = model.PortfolioServices.FeaturedInsighPdf;
            portfolioServiceData.InsightHeading = model.PortfolioServices.InsightHeading;

            // Audit info
            portfolioServiceData.ModifiedDate = DateTime.Now;
            portfolioServiceData.ModifiedBy = userId;

            // ✅ Handle brochure upload
            if (brochure != null && brochure.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", DbImagePath.PortfolioServicesImage);
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string originalFileName = Path.GetFileName(brochure.FileName);
                string filePath = Path.Combine(uploadsFolder, originalFileName);

                // Ensure unique file name
                if (File.Exists(filePath))
                {
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(originalFileName);
                    string extension = Path.GetExtension(originalFileName);
                    string randomSuffix = "_" + Guid.NewGuid().ToString("N")[..6];
                    string newFileName = fileNameWithoutExt + randomSuffix + extension;
                    filePath = Path.Combine(uploadsFolder, newFileName);
                    originalFileName = newFileName;
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await brochure.CopyToAsync(stream);
                }

                portfolioServiceData.FeaturedInsighPdf = originalFileName;
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
