using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IGS.Web.ViewComponents
{
    public class TeamBioSectionViewComponent : ViewComponent
    {
        private readonly ITeamVmService _teamVmService;

        public TeamBioSectionViewComponent(ITeamVmService teamVmService)
        {
            _teamVmService = teamVmService;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            int teamId,
            string? categoryIds = null,
            string? locationIds = null,
            string? orderBy = "DisplayOrder")
        {
            // Fetch the team detail using existing service
            var teamDetail = await _teamVmService.GetTeamModelAsync(teamId, categoryIds, locationIds, orderBy);

            if (teamDetail?.TeamInfo?.TeamId == null)
            {
                return Content("<p>Team member not found.</p>");
            }

            // Create the lightweight model specifically for the bio modal
            var bioModel = new TeamBioModel(teamDetail.TeamInfo);

            return View("_TeamBio", bioModel);
        }
    }
}
