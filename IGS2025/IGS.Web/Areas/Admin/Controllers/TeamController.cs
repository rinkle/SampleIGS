using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models;
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
    public class TeamController : BaseController
    {
        private readonly ITeamVmService _teamVmService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly string _baseUrl;

        public TeamController(
            IOptions<AppSettings> options,
            ILoggerService logger,
            ITeamVmService teamVmService,
            IUnitOfWork unitOfWork)
        {
            _baseUrl = options.Value.BaseUrl;
            _logger = logger;
            _teamVmService = teamVmService;
            _unitOfWork = unitOfWork;
        }

        // GET: /Admin/Team
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = await _teamVmService.GetTeamVmAsync(null, null, null, isAdmin: true);
            return View(vm);
        }

        // GET: /Admin/Team/ManageTeam/5
        [HttpGet]
        public async Task<IActionResult> ManageTeam(string id)
        {
            if (!int.TryParse(id, out var teamId))
                return BadRequest("Invalid Team Id");

            var vm = await _teamVmService.GetTeamModelAsync(teamId);
            return View(vm);
        }

        // POST: /Admin/Team/SaveTeamData
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveTeamData(TeamModel model)
        {
            if (model == null)
                return BadRequest("Invalid data submitted");

            try
            {
                // check existing
                var teamEntity = await _unitOfWork.Team.GetAsync(
                    t => t.TeamId == model.TeamInfo.TeamId,
                    tracked: true);

                bool isNew = false;

                if (teamEntity == null)
                {
                    teamEntity = new Team
                    {
                        CreatedBy = User?.Identity?.Name ?? "system",
                        CreatedDate = DateTime.Now,
                        IsActive = true
                    };
                    isNew = true;
                }

                // map fields
                teamEntity.Fk_LocationId = model.TeamInfo.Fk_LocationId;
                teamEntity.Fk_TeamTitleId = model.TeamInfo.Fk_TeamTitleId;
                teamEntity.FirstName = model.TeamInfo.FirstName;
                teamEntity.MiddleName = model.TeamInfo.MiddleName;
                teamEntity.LastName = model.TeamInfo.LastName;
                teamEntity.Email = model.TeamInfo.Email;
                teamEntity.OfficeNumber = model.TeamInfo.OfficeNumber;
                teamEntity.PhoneNumber = model.TeamInfo.PhoneNumber;
                teamEntity.LinkedInUrl = model.TeamInfo.LinkedInUrl;
                teamEntity.BioImage = model.TeamInfo.BioImage;
                teamEntity.GridImage = model.TeamInfo.GridImage;
                teamEntity.HomeBioImage = model.TeamInfo.HomeBioImage;
                teamEntity.Comments = model.TeamInfo.Comments;
                teamEntity.SortDescription = model.TeamInfo.SortDescription;
                teamEntity.Description = model.TeamInfo.Description;
                teamEntity.EducationTitle = model.TeamInfo.EducationTitle;
                teamEntity.EducationDescription = model.TeamInfo.EducationDescription;
                teamEntity.ExperienceTitle = model.TeamInfo.ExperienceTitle;
                teamEntity.ExperienceDescription = model.TeamInfo.ExperienceDescription;
                teamEntity.ListOnHome = model.TeamInfo.ListOnHome;
                teamEntity.DisplayOrder = model.TeamInfo.DisplayOrder;
                teamEntity.VCard = model.TeamInfo.VCard;

                teamEntity.IsActive = model.TeamInfo.IsActive ?? true;
                teamEntity.ModifiedDate = DateTime.Now;
                teamEntity.ModifiedBy = User?.Identity?.Name ?? "system";

                // ✅ Save correctly
                if (isNew)
                    await _unitOfWork.Team.AddAsync(teamEntity);
                else
                    _unitOfWork.Team.Update(teamEntity);

                await _unitOfWork.SaveAsync();

                SuccessNotification("Team saved successfully.");
                return Redirect(_baseUrl + "admin/team/");
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error saving Team");
                ErrorNotification("Error while saving Team: " + ex.Message);
                return Redirect(_baseUrl + "admin/team/");
            }
        }

    }
}
