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
        private readonly string baseUrl;
        private readonly ILoggerService _logger;
        private readonly IPortfolioServicesVmService _portfolioServicesVmService;

        public PortfolioServicesController(IOptions<AppSettings> options, ILoggerService logger, IPortfolioServicesVmService portfolioServicesVmService)
        {
            baseUrl = options.Value.BaseUrl;
            _logger = logger;
            _portfolioServicesVmService = portfolioServicesVmService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var vm = await _portfolioServicesVmService.GetPortfolioServicesVmAsync(true);
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
        public async Task<IActionResult> SavePortfolioServiceData(PortfolioServicesViewModel model, IFormFile? Brochure)
        {
            try
            {
                var userId = User?.Identity is ClaimsIdentity identity
                    ? identity.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    : null;

                await _portfolioServicesVmService.SavePortfolioServicesAsync(model, Brochure, userId);

                SuccessNotification(Message.SuccessMessage);
                return Redirect(baseUrl + "admin/portfolioservices/");
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "Error in PortfolioServices/SavePortfolioServiceData");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
                return View("Index", model);
            }
        }
    }
}
