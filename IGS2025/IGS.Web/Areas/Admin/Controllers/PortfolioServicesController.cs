using Globalsetting;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;
using IGS.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace IGS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = (UserRoles.Admin + "," + UserRoles.SuperAdmin))]
    [RemoveCache]
    public class PortfolioServicesController : BaseController
    {
        private readonly string _baseUrl;
        private readonly ILoggerService _logger;
        private readonly IPortfolioServicesVmService _portfolioVmService;
        private readonly IPortfolioServicesService _portfolioService;
        private readonly ICommonListingService _commonListingService;

        public PortfolioServicesController(
            IOptions<AppSettings> options,
            ILoggerService logger,
            IPortfolioServicesVmService portfolioVmService,
            IPortfolioServicesService portfolioService,
            ICommonListingService commonListingService)
        {
            _baseUrl = options.Value.BaseUrl;
            _logger = logger;
            _portfolioVmService = portfolioVmService;
            _portfolioService = portfolioService;
            _commonListingService = commonListingService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var vm = await _portfolioVmService.GetPortfolioServicesVmAsync(true);
                return View(vm);
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "Error in PortfolioServices/Index");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
                return View(null);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePortfolioServiceData(PortfolioServicesViewModel model)
        {
            try
            {
                if (model.PortfolioServices != null)
                {
                    await _portfolioService.SavePortfolioServiceAsync(model);
                    SuccessNotification("Portfolio Services data saved successfully!");
                    return Redirect(_baseUrl + "admin/portfolioservices/");
                }
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "Error in PortfolioServices/SavePortfolioServiceData");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
            }

            return View("Index", model);
        }
    }
}
