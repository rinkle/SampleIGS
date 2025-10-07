using Globalsetting;
using IGS.Dal.Services.Implementations;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;
using IGS.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IGS.Web.Controllers
{
    public class TeamController : BaseController
    {
        private readonly string _baseUrl;
        private readonly ILoggerService _logger;
        private readonly ICommonService _commonService;
        private readonly ITeamVmService _teamVmService;

        public TeamController(IOptions<AppSettings> options, ILoggerService logger, ITeamVmService teamVmService, ICommonService commonService)
        {
            _baseUrl = options.Value.BaseUrl;
            _logger = logger;
            _teamVmService = teamVmService;
            _commonService = commonService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.pageName = PageName.Team;
            ViewBag.canonical = "team";
            CommonHeaderFooterModel CommonServiceModel = await _commonService.GetCommonServiceAsync(PageName.Team);
            ViewBag.CommonService = CommonServiceModel;
            var vm = await _teamVmService.GetTeamVmAsync("-1", "-1", "DisplayOrder", isAdmin: false);
            ViewBag.ActiveCategory = "1";
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> TeamBioInfo([FromBody] int teamId)
        {
            var model = await _teamVmService.GetTeamModelAsync(teamId, null, null, "DisplayOrder");

            if (model == null || model.TeamInfo == null)
                return Content("<p>Team member not found.</p>");

            return PartialView("_TeamBio", model);
        }
    }
}
