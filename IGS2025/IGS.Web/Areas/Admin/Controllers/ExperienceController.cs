using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models;
using IGS.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IGS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = (UserRoles.Admin + "," + UserRoles.SuperAdmin))]
    [RemoveCache]
    public class ExperienceController : BaseController
    {
        private readonly IExperienceVmService _experienceVmService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly string _baseUrl;

        public ExperienceController(
            IOptions<AppSettings> options,
            ILoggerService logger,
            IExperienceVmService experienceVmService,
            IUnitOfWork unitOfWork)
        {
            _baseUrl = options.Value.BaseUrl;
            _logger = logger;
            _experienceVmService = experienceVmService;
            _unitOfWork = unitOfWork;
        }

        // GET: /Admin/Experience
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = await _experienceVmService.GetExperienceVmAsync(null, null, null, isAdmin: false);
            return View(vm);
        }

        // GET: /Admin/Experience/ManageExperience/5
        [HttpGet]
        public async Task<IActionResult> ManageExperience(string id)
        {
            if (!int.TryParse(id, out var experienceId))
                return BadRequest("Invalid Experience Id");

            var vm = await _experienceVmService.GetExperienceModelAsync(experienceId);
            return View(vm);
        }

        // POST: /Admin/Experience/SaveExperienceData
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveExperienceData(ExperienceModel model)
        {
            if (model == null || model.ExperienceInfo == null)
                return BadRequest("Invalid data submitted");

            try
            {
                // 1. Save Experience main entity
                var expEntity = await _unitOfWork.Experience.GetAsync(
                    e => e.Id == model.ExperienceInfo.Id,
                    tracked: true
                );

                if (expEntity == null)
                {
                    expEntity = new Experience();
                    await _unitOfWork.Experience.AddAsync(expEntity);
                }

                // map fields from model.ExperienceInfo → expEntity
                expEntity.ClientName = model.ExperienceInfo.ClientName;
                expEntity.TopLogo1 = model.ExperienceInfo.TopLogo1;
                expEntity.TopLogo1Caption = model.ExperienceInfo.TopLogo1Caption;
                expEntity.TopLogo2 = model.ExperienceInfo.TopLogo2;
                expEntity.TopLogo2Caption = model.ExperienceInfo.TopLogo2Caption;
                expEntity.Bottom1Logo = model.ExperienceInfo.Bottom1Logo;
                expEntity.Bottom1LogoCaption = model.ExperienceInfo.Bottom1LogoCaption;
                expEntity.Bottom2Logo = model.ExperienceInfo.Bottom2Logo;
                expEntity.Bottom2LogoCation = model.ExperienceInfo.Bottom2LogoCation;
                expEntity.Target = model.ExperienceInfo.Target;
                expEntity.Website = model.ExperienceInfo.Website;
                expEntity.DisplayOrder = model.ExperienceInfo.DisplayOrder;
                expEntity.TransactionDate = model.ExperienceInfo.TransactionDate;
                expEntity.EndDate = model.ExperienceInfo.EndDate;
                expEntity.HideTombstone = model.ExperienceInfo.HideTombstone;
                expEntity.IsActive = model.ExperienceInfo.IsActive ?? true;
                expEntity.ModifiedDate = DateTime.Now;
                expEntity.ModifiedBy = User?.Identity?.Name ?? "system";

                _unitOfWork.Experience.Update(expEntity);
                await _unitOfWork.SaveAsync();

                // 2. Replace Industry Mappings
                if (model.ExperienceIndustryCategoryMapping?.Any() == true)
                {
                    var selectedIds = model.ExperienceIndustryCategoryMapping
                        .Where(x => x.CheckedStatus == true)
                        .Select(x => x.Id)
                        .ToList();

                    await _unitOfWork.Experience.ReplaceIndustryMappingsAsync(expEntity.Id, selectedIds);
                }

                // 3. Replace Page Mappings
                if (model.ExperiencePageMapping?.Any() == true)
                {
                    var selectedIds = model.ExperiencePageMapping
                        .Where(x => x.CheckedStatus == true)
                        .Select(x => x.Id)
                        .ToList();

                    await _unitOfWork.Experience.ReplacePageMappingsAsync(expEntity.Id, selectedIds);
                }

                SuccessNotification("Experience saved successfully.");
                return Redirect(_baseUrl + "admin/experience");
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error saving Experience");
                ErrorNotification("Error while saving Experience: " + ex.Message);
                return Redirect(_baseUrl + "admin/experience");
            }
        }
    }
}
