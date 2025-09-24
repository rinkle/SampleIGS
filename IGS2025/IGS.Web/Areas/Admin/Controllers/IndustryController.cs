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
    public class IndustryController : BaseController
    {
        private readonly string baseUrl;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly IIndustryCategoryService _iIndustryCategoryService;
        public IndustryController(IOptions<AppSettings> options, IUnitOfWork unitOfWork, ILoggerService logger, IIndustryCategoryService iIndustryCategoryService)
        {
            baseUrl = options.Value.BaseUrl;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _iIndustryCategoryService = iIndustryCategoryService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var IndustryResult = await _unitOfWork.IndustryService.GetIndustryFromSpAsync();
                var allIndustryCategories = await _unitOfWork.IndustryService.GetIndustryCategoryFromSpAsync();
                var vm = new IndustryViewModel(IndustryResult, allIndustryCategories, true);
                return View(vm);
            }
            catch (Exception Ex)
            {
                int errorId = await _logger.LogErrorAsync(Ex, "Error in Industry/Index");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
            }
            return View(null);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveIndustryData(IndustryViewModel model)
        {
            try
            {
                if (model.Industry != null)
                {
                    #region Save IndustryCategory
                    if (model.IndustryCategory != null && model.IndustryCategory.Count > 0)
                    {
                        await _iIndustryCategoryService.SaveIndustryCategoryAsync(model.IndustryCategory);
                    }
                    #endregion

                    #region Industry
                    var industryData = await _unitOfWork.IndustryService
                        .GetAsync(h => h.Id == model.Industry.Id, tracked: true);

                    if (industryData != null)
                    {
                        // Update fields
                        industryData.IndustryHeading = model.Industry.IndustryHeading;
                        industryData.IndustryDescription = model.Industry.IndustryDescription;
                        industryData.InsightHeading = model.Industry.InsightHeading;
                        industryData.ModifiedDate = DateTime.Now;

                        // Always log who modified
                        industryData.ModifiedBy = User?.Identity is ClaimsIdentity identity
                            ? identity.FindFirst(ClaimTypes.NameIdentifier)?.Value
                            : "System";

                        await _unitOfWork.SaveAsync();

                        SuccessNotification("Industry page data saved successfully!");
                        return Redirect(baseUrl + "admin/industry/");
                    }
                    else
                    {
                        ErrorNotification("Industry record not found.");
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "Error in Industry/SaveIndustryData");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
            }

            // Show same page if failure
            return View("Index", model);
        }



    }

}
