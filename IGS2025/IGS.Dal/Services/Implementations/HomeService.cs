using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace IGS.Dal.Services.Implementations
{
    public class HomeService : IHomeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly ICommonListingService _commonListingService;
        private readonly GlobalEnvironmentSetting _globalEnvironment;

        public HomeService(
            IUnitOfWork unitOfWork,
            ILoggerService logger,
            ICommonListingService commonListingService,
            GlobalEnvironmentSetting globalEnvironment)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _commonListingService = commonListingService;
            _globalEnvironment = globalEnvironment;
        }

        public async Task SaveHomeAsync(HomeViewModel model)
        {
            try
            {
                if (model?.Home == null) return;

                // ✅ Save Common Listings
                if (model.Carousel?.Count > 0)
                    await _commonListingService.SaveCommonListingAsync(model.Carousel);

                if (model.AtAGlance?.Count > 0)
                    await _commonListingService.SaveCommonListingAsync(model.AtAGlance);

                if (model.CoreAreasoFocus?.Count > 0)
                    await _commonListingService.SaveCommonListingAsync(model.CoreAreasoFocus);

                if (model.TransactionsandGrowth?.Count > 0)
                    await _commonListingService.SaveCommonListingAsync(model.TransactionsandGrowth);

                var homeData = await _unitOfWork.Home.GetAsync(h => h.Id == model.Home.Id, tracked: true);
                if (homeData == null) return;

                // ✅ Map fields
                homeData.TransactionsGrowthHeading = model.Home.TransactionsGrowthHeading;
                homeData.TransactionsGrowthDescription = model.Home.TransactionsGrowthDescription;
                homeData.CoreAreasHeading = model.Home.CoreAreasHeading;
                homeData.CoreAreaDescription = model.Home.CoreAreaDescription;
                homeData.RecentProjectsHeading = model.Home.RecentProjectsHeading;
                homeData.RecentProjectsDescription = model.Home.RecentProjectsDescription;
                homeData.InsightTitle = model.Home.InsightTitle;
                homeData.InsightHeading = model.Home.InsightHeading;
                homeData.InsightDescription = model.Home.InsightDescription;
                homeData.InsightImage = model.Home.InsightImage;
                homeData.InsightPdfReport = model.Home.InsightPdfReport;
                homeData.NewsletterHeading = model.Home.NewsletterHeading;
                homeData.NewsletterScript = model.Home.NewsletterScript;
                homeData.CareersUrl = model.Home.CareersUrl;
                homeData.InvestorLogin = model.Home.InvestorLogin;
                homeData.VimeoVideoUrl = model.Home.VimeoVideoUrl;
                homeData.LinkedInUrl = model.Home.LinkedInUrl;
                homeData.TwitterUrl = model.Home.TwitterUrl;
                homeData.FacebookUrl = model.Home.FacebookUrl;
                homeData.Email = model.Home.Email;
                homeData.OverviewPdf = model.Home.OverviewPdf;
                homeData.WebsiteUpdateDate = model.Home.WebsiteUpdateDate;
                homeData.ModifiedDate = DateTime.Now;
                homeData.ModifiedBy = _globalEnvironment.UserId;
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in HomeService.SaveHomeAsync");
                throw; // rethrow so controller can decide how to handle
            }
        }
    }
}
