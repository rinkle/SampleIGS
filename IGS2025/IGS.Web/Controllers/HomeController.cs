using Globalsetting;
using IGS.Dal.Services.Implementations;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;
using IGS.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;


namespace IGS.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly string _baseUrl;
        private readonly ILoggerService _logger;
        private readonly IHomeVmService _homeVmService;
        private readonly IHomeService _homeService;
        private readonly ICommonService _commonService;
        private readonly IExperienceVmService _experienceVmService;
        private readonly ITransactionServicesVmService _transactionVmService;
        private readonly IPortfolioServicesVmService _portfolioVmService;
        private readonly IIndustryVmService _industryVmService;

        public HomeController(IOptions<AppSettings> options, ILoggerService logger, IHomeVmService homeVmService, IHomeService homeService, ICommonService commonService, IExperienceVmService experienceVmService, ITransactionServicesVmService transactionVmService, IPortfolioServicesVmService portfolioServicesVmService, IIndustryVmService industryVmService)
        {
            _baseUrl = options.Value.BaseUrl;
            _logger = logger;
            _homeVmService = homeVmService;
            _homeService = homeService;
            _commonService = commonService;
            _experienceVmService = experienceVmService;
            _transactionVmService = transactionVmService;
            _portfolioVmService = portfolioServicesVmService;
            _industryVmService = industryVmService;
        }
        #region Home Page
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.pageName = PageName.Home;
                ViewBag.canonical = string.Empty;
                CommonHeaderFooterModel CommonServiceModel = await _commonService.GetCommonServiceAsync(PageName.Home);
                ViewBag.CommonService = CommonServiceModel;
                HomeViewModel vm = await _homeVmService.GetHomeVmAsync(isAdmin: false);
                return View(vm);
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "Error in Home/Index");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
                return View(null);
            }
        }
        #endregion


        #region Transaction Services Page
        [Route("transaction-services")]
        public async Task<IActionResult> TransactionServices()
        {
            try
            {
                ViewBag.pageName = PageName.TransactionServices;
                ViewBag.canonical = "transaction-services";
                CommonHeaderFooterModel CommonServiceModel = await _commonService.GetCommonServiceAsync(PageName.TransactionServices);
                ViewBag.CommonService = CommonServiceModel;
                TransactionServicesViewModel vm = await _transactionVmService.GetTransactionServicesVmAsync(isAdmin: false);
                return View(vm);
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "Error in Home/Index");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
                return View(null);
            }
        }
        #endregion

        #region Portfolio Services Page
        [Route("portfolio-services")]
        public async Task<IActionResult> PortfolioServices()
        {
            try
            {
                ViewBag.pageName = PageName.PortfolioServices;
                ViewBag.canonical = "portfolio-services";
                CommonHeaderFooterModel CommonServiceModel = await _commonService.GetCommonServiceAsync(PageName.PortfolioServices);
                ViewBag.CommonService = CommonServiceModel;
                PortfolioServicesViewModel vm = await _portfolioVmService.GetPortfolioServicesVmAsync(isAdmin: false);
                return View(vm);
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "Error in Home/Index");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
                return View(null);
            }
        }
        #endregion

        #region industries Page
        [Route("industries")]
        public async Task<IActionResult> Industries()
        {
            try
            {
                ViewBag.pageName = PageName.Industries;
                ViewBag.canonical = "industries";
                CommonHeaderFooterModel CommonServiceModel = await _commonService.GetCommonServiceAsync(PageName.Industries);
                ViewBag.CommonService = CommonServiceModel;
                IndustryViewModel vm = await _industryVmService.GetIndustryVmAsync(isAdmin: false);
                return View(vm);
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "Error in Home/Index");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
                return View(null);
            }
        }
        #endregion
        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
