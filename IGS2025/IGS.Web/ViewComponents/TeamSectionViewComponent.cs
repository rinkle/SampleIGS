using IGS.Dal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IGS.Web.ViewComponents
{
    public class TeamSectionViewComponent : ViewComponent
    {
        private readonly ITeamVmService _teamVmService;

        public TeamSectionViewComponent(ITeamVmService teamVmService)
        {
            _teamVmService = teamVmService;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            string? categoryIds = null,
            string? locationIds = null,
            string? orderBy = "DisplayOrder",
            bool? isAdmin = false)
        {
            var vm = await _teamVmService.GetTeamVmAsync(
                categoryIds,
                locationIds,
                orderBy,
                isAdmin ?? false);

            return View("_TeamSectionPartial", vm);
        }
    }
}
