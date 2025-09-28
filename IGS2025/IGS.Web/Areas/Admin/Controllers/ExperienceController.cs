using Globalsetting;
using IGS.Dal.Repository;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly GlobalEnvironmentSetting _globalEnvironment;

        public ExperienceController(
            IOptions<AppSettings> options,
            ILoggerService logger,
            IExperienceVmService experienceVmService,
            IExperienceService experienceService, IUnitOfWork unitOfWork,
            GlobalEnvironmentSetting globalEnvironment)   // ✅ inject service
        {
            _baseUrl = options.Value.BaseUrl;
            _logger = logger;
            _experienceVmService = experienceVmService;
            _experienceService = experienceService;
            _unitOfWork = unitOfWork;
            _globalEnvironment = globalEnvironment;
        }

        // GET: /Admin/Experience
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = await _experienceVmService.GetExperienceVmAsync(null, null, null, isAdmin: false);
            return View(vm);
        }

        #region Manage Experience
        // GET: /Admin/Experience/ManageExperience/5
        [HttpGet]
        public async Task<IActionResult> ManageExperience(string id)
        {

            if (!int.TryParse(id, out var experienceId))
                return BadRequest("Invalid Experience Id");
            ViewBag.ExperienceId = experienceId;
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
                return Redirect(_baseUrl + "admin/experience/");
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error saving Experience");
                ErrorNotification("Error while saving Experience: " + ex.Message);
                return Redirect(_baseUrl + "admin/experience/");
            }
        }

        #endregion

        #region delete logo
        [HttpPost]
        public async Task<IActionResult> DeleteTopLogo1(int id)
        {
            bool success = await _experienceService.DeleteLogoAsync(id, exp => exp.TopLogo1 = null);
            return Json(new { isSuccess = success, message = success ? Message.DeleteSuccessMessage : Message.DataNotFoundMessage });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTopLogo2(int id)
        {
            bool success = await _experienceService.DeleteLogoAsync(id, exp => exp.TopLogo2 = null);
            return Json(new { isSuccess = success, message = success ? Message.DeleteSuccessMessage : Message.DataNotFoundMessage });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBottomLogo1(int id)
        {
            bool success = await _experienceService.DeleteLogoAsync(id, exp => exp.Bottom1Logo = null);
            return Json(new { isSuccess = success, message = success ? Message.DeleteSuccessMessage : Message.DataNotFoundMessage });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBottomLogo2(int id)
        {
            bool success = await _experienceService.DeleteLogoAsync(id, exp => exp.Bottom2Logo = null);
            return Json(new { isSuccess = success, message = success ? Message.DeleteSuccessMessage : Message.DataNotFoundMessage });
        }

        #endregion

        #region delete Experience
        [HttpPost]
        public async Task<IActionResult> DeleteExperience(int id)
        {
            bool status = false;
            string returnMessage;

            try
            {
                status = await _experienceService.DeleteExperienceAsync(id);
                returnMessage = status ? Message.DeleteSuccessMessage : Message.DataNotFoundMessage;
            }
            catch
            {
                returnMessage = "An error occurred while deleting the experience.";
            }

            return Json(new { isSuccess = status, message = returnMessage });
        }
        #endregion
    }
}
