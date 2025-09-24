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
    public class HomeController : BaseController
    {
        private readonly string _baseUrl;
        private readonly ILoggerService _logger;
        private readonly IHomeVmService _homeVmService;
        private readonly IHomeService _homeService;

        public HomeController(
            IOptions<AppSettings> options,
            ILoggerService logger,
            IHomeVmService homeVmService,
            IHomeService homeService)
        {
            _baseUrl = options.Value.BaseUrl;
            _logger = logger;
            _homeVmService = homeVmService;
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var vm = await _homeVmService.GetHomeVmAsync(isAdmin: true);
                return View(vm);
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "Error in Home/Index");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
                return View(null);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveHomeData(HomeViewModel model, IFormFile? brochure)
        {
            try
            {
                string userId = User?.Identity is ClaimsIdentity identity
                    ? identity.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "system"
                    : "system";

                await _homeService.SaveHomeAsync(model, brochure, userId);

                SuccessNotification("Home page data saved successfully!");
                return Redirect(_baseUrl + "admin/home/");
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "Error in Home/SaveHomeData");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
                return View("Index", model);
            }
        }
    }
}
