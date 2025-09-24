using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;
using IGS.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Buffers.Text;
using System.Security.Claims;

namespace IGS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = (UserRoles.Admin + "," + UserRoles.SuperAdmin))]
    [RemoveCache]
    public class PortfolioServicesController : BaseController
    {
        private readonly string baseUrl;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly ICommonListingService _commonListingService;
        public PortfolioServicesController(IOptions<AppSettings> options, IUnitOfWork unitOfWork, ILoggerService logger, ICommonListingService commonListingService)
        {
            baseUrl = options.Value.BaseUrl;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _commonListingService = commonListingService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var portfolioServiceData = await _unitOfWork.PortfolioService.GetPortfolioServiceFromSpAsync();
                var allListings = await _unitOfWork.CommonListing.GetCommonListingFromSpAsync((int)PageEnum.PortfolioServices);
                var vm = new PortfolioServicessViewModel(portfolioServiceData, allListings.ToList(), true);
                return View(vm);
            }
            catch (Exception Ex)
            {
                int errorId = await _logger.LogErrorAsync(Ex, "Error in PortfolioServices/Index");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
            }
            return View(null);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePortfolioServiceData(PortfolioServicessViewModel model, IFormFile? Brochure)
        {
            try
            {
                if (model.PortfolioServices != null)
                {
                    await _commonListingService.SaveCommonListingAsync(model.CoreAreasofFocus);
                    var portfolioServiceData = await _unitOfWork.PortfolioService.GetAsync(h => h.Id == model.PortfolioServices.Id, tracked: true);
                    if (portfolioServiceData != null)
                    {
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
                        portfolioServiceData.ModifiedDate = DateTime.Now;
                        portfolioServiceData.ModifiedBy = User?.Identity is ClaimsIdentity identity
                            ? identity.FindFirst(ClaimTypes.NameIdentifier)?.Value
                            : null;
                        await _unitOfWork.SaveAsync();
                        SuccessNotification(Message.SuccessMessage);
                        return Redirect(baseUrl + "admin/portfolioservices/");
                    }
                }
            }
            catch (Exception Ex)
            {
                int errorId = await _logger.LogErrorAsync(Ex, "Error in PortfolioServices/SavePortfolioServicesData");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
            }

            return View("Index", model);
        }


    }

}
