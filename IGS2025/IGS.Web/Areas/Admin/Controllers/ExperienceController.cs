using Globalsetting;
using IGS.Dal.Services.Implementations;
using IGS.Dal.Services.Interfaces;
using IGS.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Buffers.Text;

namespace IGS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = (UserRoles.Admin + "," + UserRoles.SuperAdmin))]
    [RemoveCache]
    public class ExperienceController : BaseController
    {
        private readonly IExperienceVmService _experienceVmService;
        private readonly string _baseUrl;
        private readonly ILoggerService _logger;

        public ExperienceController(IOptions<AppSettings> options, ILoggerService logger, IExperienceVmService experienceVmService)
        {
            _baseUrl = options.Value.BaseUrl;
            _logger = logger;
            _experienceVmService = experienceVmService;
        }

        // GET: /Admin/Experience
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Admin area → isAdmin = true
            var vm = await _experienceVmService.GetExperienceVmAsync(null, null, null, isAdmin: false);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> ManageExperience(string id)
        {
            if (!int.TryParse(id, out var experienceId))
                return BadRequest("Invalid Experience Id");

            var vm = await _experienceVmService.GetExperienceModelAsync(experienceId);
            return View(vm);
        }
    }
}
