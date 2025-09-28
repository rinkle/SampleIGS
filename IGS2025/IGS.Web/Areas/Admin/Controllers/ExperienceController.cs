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

        [HttpPost]
        public async Task<IActionResult> DeleteTopLogo1(int id)
        {
            return await DeleteLogoAsync(id, exp => exp.TopLogo1 = null);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTopLogo2(int id)
        {
            return await DeleteLogoAsync(id, exp => exp.TopLogo2 = null);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBottomLogo1(int id)
        {
            return await DeleteLogoAsync(id, exp => exp.Bottom1Logo = null);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBottomLogo2(int id)
        {
            return await DeleteLogoAsync(id, exp => exp.Bottom2Logo = null);
        }

        // 🔑 Shared private method with explicit lambda
        private async Task<IActionResult> DeleteLogoAsync(int id, Action<Experience> clearLogoAction)
        {
            bool status = false;
            string returnMessage;

            try
            {
                var experience = await _unitOfWork.Experience.GetAsync(
                    e => e.Id == id,
                    tracked: true);

                if (experience != null)
                {
                    // Run the action (set TopLogo1/TopLogo2/etc. to null)
                    clearLogoAction(experience);

                    experience.ModifiedBy = _globalEnvironment.UserId;
                    experience.ModifiedDate = DateTime.Now;

                    _unitOfWork.Experience.Update(experience);
                    await _unitOfWork.SaveAsync();

                    status = true;
                    returnMessage = Message.DeleteSuccessMessage;
                }
                else
                {
                    returnMessage = Message.DataNotFoundMessage;
                }
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, $"Error deleting logo for Experience {id}");
                returnMessage = "An error occurred while deleting the logo.";
            }

            return Json(new { IsSuccess = status, Message = returnMessage });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteExperience(int id)
        {
            bool status = false;
            string returnMessage;

            try
            {
                var experience = await _unitOfWork.Experience.GetAsync(
                    x => x.Id == id,
                    tracked: true);

                if (experience != null)
                {
                    experience.IsActive = false;
                    experience.ModifiedBy = _globalEnvironment.UserId;
                    experience.ModifiedDate = DateTime.Now;

                    _unitOfWork.Experience.Update(experience);
                    await _unitOfWork.SaveAsync();

                    status = true;
                    returnMessage = Message.DeleteSuccessMessage;
                }
                else
                {
                    returnMessage = Message.DataNotFoundMessage;
                }
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, $"Error deleting Experience {id}");
                returnMessage = "An error occurred while deleting the experience.";
            }

            return Json(new { isSuccess = status, message = returnMessage });
        }





    }
}
