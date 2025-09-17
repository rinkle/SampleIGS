using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services;
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
    public class HomeController : BaseController
    {
        private readonly string baseUrl;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly ICommonListingService _commonListingService;
        public HomeController(IOptions<AppSettings> options, IUnitOfWork unitOfWork, ILoggerService logger, ICommonListingService commonListingService)
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
                var homeResult = await _unitOfWork.Home.GetHomeFromSpAsync();
                var allListings = await _unitOfWork.CommonListing.GetCommonListingFromSpAsync((int)PageEnum.Home);
                var vm = new HomeViewModel(homeResult, allListings.ToList(),true);
                return View(vm);
            }
            catch (Exception Ex)
            {
                int errorId = await _logger.LogErrorAsync(Ex, "Error in Home/Index");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
            }
            return View(null);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveHomeData(HomeViewModel model, IFormFile? Brochure)
        {
            try
            {
                if (model.Home != null)
                {
                    await _commonListingService.SaveCommonListingAsync(model.Carousel);
                    await _commonListingService.SaveCommonListingAsync(model.AtAGlance);

                    var homeData = await _unitOfWork.Home.GetAsync(h => h.Id == model.Home.Id, tracked: true);
                    if (homeData != null)
                    {
                        homeData.TransactionsGrowthHeading = model.Home.TransactionsGrowthHeading;
                        homeData.TransactionsGrowthDescription = model.Home.TransactionsGrowthDescription;
                        homeData.CoreAreasHeading = model.Home.CoreAreasHeading;
                        homeData.CoreAreaDescription = model.Home.CoreAreaDescription;
                        homeData.InsightHeading = model.Home.InsightHeading;
                        homeData.InsightDescription = model.Home.InsightDescription;
                        homeData.InsightImage = model.Home.InsightImage;
                        homeData.InsightPdfReport = model.Home.InsightPdfReport;
                        homeData.NewsletterHeading = model.Home.NewsletterHeading;
                        homeData.NewsletterScript = model.Home.NewsletterScript;
                        homeData.InvestorLogin = model.Home.InvestorLogin;
                        homeData.VimeoVideoUrl = model.Home.VimeoVideoUrl;
                        homeData.LinkedInUrl = model.Home.LinkedInUrl;
                        homeData.TwitterUrl = model.Home.TwitterUrl;
                        homeData.FacebookUrl = model.Home.FacebookUrl;
                        homeData.Email = model.Home.Email;
                        homeData.OverviewPdf = model.Home.OverviewPdf;
                        homeData.WebsiteUpdateDate = model.Home.WebsiteUpdateDate;
                        homeData.ModifiedDate = DateTime.Now;
                        homeData.ModifiedBy = User?.Identity is ClaimsIdentity identity
                            ? identity.FindFirst(ClaimTypes.NameIdentifier)?.Value
                            : null;

                        #region save Brochure
                        if (Brochure != null && Brochure.Length > 0)
                        {
                            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", DbImagePath.HomeImage);
                            if (!Directory.Exists(uploadsFolder))
                                Directory.CreateDirectory(uploadsFolder);

                            // Get original filename
                            string originalFileName = Path.GetFileName(Brochure.FileName);
                            string filePath = Path.Combine(uploadsFolder, originalFileName);

                            // If file exists, append a random suffix before extension
                            if (System.IO.File.Exists(filePath))
                            {
                                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(originalFileName);
                                string extension = Path.GetExtension(originalFileName);
                                string randomSuffix = "_" + Guid.NewGuid().ToString("N").Substring(0, 6); // short random string
                                string newFileName = fileNameWithoutExt + randomSuffix + extension;
                                filePath = Path.Combine(uploadsFolder, newFileName);
                                originalFileName = newFileName;
                            }

                            // Save the file
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await Brochure.CopyToAsync(stream);
                            }

                            // Save final filename to DB
                            homeData.OverviewPdf = originalFileName;
                        }
                        #endregion


                        await _unitOfWork.SaveAsync();
                        SuccessNotification("Home page data saved successfully!");
                        return Redirect(baseUrl + "admin/home/");
                    }
                }
            }
            catch (Exception Ex)
            {
                int errorId = await _logger.LogErrorAsync(Ex, "Error in Home/SaveHomeData");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
            }

            return View("Index", model);
        }


    }

}
