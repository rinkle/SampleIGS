using Globalsetting;
using IGS.Dal.Services.Interfaces;
using IGS.Models;
using IGS.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;

namespace IGS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = (UserRoles.Admin + "," + UserRoles.SuperAdmin))]
    [RemoveCache]
    public class TeamController : BaseController
    {
        private readonly ITeamVmService _teamVmService;
        private readonly ITeamService _teamService;
        private readonly ILoggerService _logger;
        private readonly string _baseUrl;

        public TeamController(
            IOptions<AppSettings> options,
            ILoggerService logger,
            ITeamVmService teamVmService,
            ITeamService teamService)
        {
            _baseUrl = options.Value.BaseUrl;
            _logger = logger;
            _teamVmService = teamVmService;
            _teamService = teamService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = await _teamVmService.GetTeamVmAsync(orderBy: "DisplayOrder", isAdmin: true);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> ManageTeam(int id = 0)
        {
            ViewBag.teamId = id;
            var model = id > 0
                ? await _teamVmService.GetTeamModelAsync(id)
                : new TeamModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveTeamData(TeamModel model)
        {
            if (!ModelState.IsValid)
            {
                ErrorNotification("Invalid Team data.");
                return Redirect(_baseUrl + "admin/team");
            }

            try
            {
                var teamId = await _teamService.SaveTeamAsync(model);

                if (teamId > 0)
                    SuccessNotification("Team saved successfully.");
                else
                    ErrorNotification("Failed to save Team.");

                return Redirect(_baseUrl + "admin/team/");
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error saving Team");
                ErrorNotification("Error while saving Team.");
                return Redirect(_baseUrl + "admin/team/");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            try
            {
                var success = await _teamService.DeleteTeamAsync(id);
                var message = success ? Message.DeleteSuccessMessage : Message.DataNotFoundMessage;

                return Json(new { isSuccess = success, message });
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, $"Error deleting Team {id}");
                return Json(new { isSuccess = false, message = "An error occurred while deleting the team." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBioImage(int id)
        {
            return await DeletePhotoAsync(id, t => t.BioImage = null);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGridImage(int id)
        {
            return await DeletePhotoAsync(id, t => t.GridImage = null);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteHomeBioImage(int id)
        {
            return await DeletePhotoAsync(id, t => t.HomeBioImage = null);
        }

        private async Task<IActionResult> DeletePhotoAsync(int id, Action<Team> clearPhotoAction)
        {
            try
            {
                var success = await _teamService.DeletePhotoAsync(id, clearPhotoAction);
                var message = success ? Message.DeleteSuccessMessage : Message.DataNotFoundMessage;

                return Json(new { isSuccess = success, message });
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, $"Error deleting photo for Team {id}");
                return Json(new { isSuccess = false, message = "An error occurred while deleting the photo." });
            }
        }
    }
}
