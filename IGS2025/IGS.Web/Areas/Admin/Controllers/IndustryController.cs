using Globalsetting;
using IGS.Dal.Services;
using IGS.Models.KeyLessModels;
using IGS.Models.ViewModels;
using IGS.ViewModels;
using IGS.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IGS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = (UserRoles.Admin + "," + UserRoles.SuperAdmin))]
    [RemoveCache]
    public class IndustryController : BaseController
    {
        private readonly string _baseUrl;
        private readonly ILoggerService _logger;
        private readonly IIndustryService _industryService;
        private readonly IIndustryVmService _industryVmService;
        private readonly IIndustryCategoryService _industryCategoryService;

        public IndustryController(
            IOptions<AppSettings> options,
            ILoggerService logger,
            IIndustryService industryService,
            IIndustryVmService industryVmService,
            IIndustryCategoryService industryCategoryService)
        {
            _baseUrl = options.Value.BaseUrl;
            _logger = logger;
            _industryService = industryService;
            _industryVmService = industryVmService;
            _industryCategoryService = industryCategoryService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var vm = await _industryVmService.GetIndustryVmAsync(isAdmin: true);
                return View(vm);
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "Error in Industry/Index");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
                return View(null);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveIndustryData(IndustryViewModel model)
        {
            try
            {
                if (model.Industry != null)
                {
                    // Save categories
                    if (model.IndustryCategory != null && model.IndustryCategory.Count > 0)
                    {
                        await _industryCategoryService.SaveIndustryCategoryAsync(model.IndustryCategory);
                    }

                    // Save industry
                    await _industryService.SaveIndustryAsync(model.Industry);
                }
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "Error in Industry/SaveIndustryData");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
            }

            // Redirect back to Index (fresh data)
            return RedirectToAction("Index");
        }
    }
}
