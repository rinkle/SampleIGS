using Globalsetting;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;
using IGS.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
namespace IGS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = (UserRoles.Admin + "," + UserRoles.SuperAdmin))]
    [RemoveCache]
    public class PrivacyPolicyController : BaseController
    {
        private readonly IPrivacyPolicyVmService _privacyPolicyVmService;
        private readonly IPrivacyPolicyService _privacyPolicyService;
        private readonly ILoggerService _logger;
        private readonly string _baseUrl;

        public PrivacyPolicyController(
            IOptions<AppSettings> options,
            ILoggerService logger,
            IPrivacyPolicyVmService privacyPolicyVmService,
            IPrivacyPolicyService privacyPolicyService)
        {
            _baseUrl = options.Value.BaseUrl;
            _logger = logger;
            _privacyPolicyVmService = privacyPolicyVmService;
            _privacyPolicyService = privacyPolicyService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var vm = await _privacyPolicyVmService.GetPrivacyPolicyVmAsync(PageName.PrivacyPolicy, true);
                return View(vm);
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "Error in PrivacyPolicy/Index");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
                return View(new PrivacyPolicyViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePrivacyPolicy(PrivacyPolicyViewModel model, string pageName = "Privacy Policy")
        {
            try
            {
                await _privacyPolicyService.SavePrivacyPolicyAsync(model, pageName);
                SuccessNotification("Privacy Policy data saved successfully!");
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "Error in PrivacyPolicy/SavePrivacyPolicy");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
            }

            return Redirect(_baseUrl + "admin/privacypolicy/");
        }
    }
}