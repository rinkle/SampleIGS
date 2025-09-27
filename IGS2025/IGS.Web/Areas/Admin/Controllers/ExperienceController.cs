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
        private readonly IExperienceService _experienceService;   // ✅ use service
        private readonly ILoggerService _logger;
        private readonly string _baseUrl;

        public ExperienceController(
            IOptions<AppSettings> options,
            ILoggerService logger,
            IExperienceVmService experienceVmService,
            IExperienceService experienceService)   // ✅ inject service
        {
            _baseUrl = options.Value.BaseUrl;
            _logger = logger;
            _experienceVmService = experienceVmService;
            _experienceService = experienceService;
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
                // ✅ delegate save to service
                await _experienceService.SaveExperienceAsync(model);

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
