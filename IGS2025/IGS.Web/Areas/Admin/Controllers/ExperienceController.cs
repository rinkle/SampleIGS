using Globalsetting;
using IGS.Dal.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IGS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = (UserRoles.Admin + "," + UserRoles.SuperAdmin))]
    [RemoveCache]
    public class ExperienceController : Controller
    {
        private readonly IExperienceVmService _experienceVmService;

        public ExperienceController(IExperienceVmService experienceVmService)
        {
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
    }
}
