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
        public async Task<IActionResult> SaveTeamData(Team model)
        {
            if (model == null)
                return BadRequest("Invalid data submitted");

            try
            {
                // fetch or create new team
                var teamEntity = await _unitOfWork.Team.GetAsync(
                    t => t.TeamId == model.TeamId,
                    tracked: true);

                if (teamEntity == null)
                {
                    teamEntity = new Team();
                    await _unitOfWork.Team.AddAsync(teamEntity);
                }

                // map fields
                teamEntity.Fk_LocationId = model.Fk_LocationId;
                teamEntity.Fk_TeamTitleId = model.Fk_TeamTitleId;
                teamEntity.FirstName = model.FirstName;
                teamEntity.MiddleName = model.MiddleName;
                teamEntity.LastName = model.LastName;
                teamEntity.Email = model.Email;
                teamEntity.OfficeNumber = model.OfficeNumber;
                teamEntity.PhoneNumber = model.PhoneNumber;
                teamEntity.LinkedInUrl = model.LinkedInUrl;
                teamEntity.BioImage = model.BioImage;
                teamEntity.GridImage = model.GridImage;
                teamEntity.HomeBioImage = model.HomeBioImage;
                teamEntity.Comments = model.Comments;
                teamEntity.SortDescription = model.SortDescription;
                teamEntity.Description = model.Description;
                teamEntity.EducationTitle = model.EducationTitle;
                teamEntity.EducationDescription = model.EducationDescription;
                teamEntity.ExperienceTitle = model.ExperienceTitle;
                teamEntity.ExperienceDescription = model.ExperienceDescription;
                teamEntity.ListOnHome = model.ListOnHome;

                // ✅ updated from OrderNo → DisplayOrder
                teamEntity.DisplayOrder = model.DisplayOrder;

                teamEntity.VCard = model.VCard;
                teamEntity.IsActive = model.IsActive ?? true;
                teamEntity.ModifiedDate = DateTime.Now;
                teamEntity.ModifiedBy = User?.Identity?.Name ?? "system";

                _unitOfWork.Team.Update(teamEntity);
                await _unitOfWork.SaveAsync();

                SuccessNotification("Team saved successfully.");
                return Redirect(_baseUrl + "admin/team");
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error saving Team");
                ErrorNotification("Error while saving Team: " + ex.Message);
                return Redirect(_baseUrl + "admin/team");
            }
        }
    }
}
