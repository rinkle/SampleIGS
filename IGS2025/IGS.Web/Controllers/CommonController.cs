using Microsoft.AspNetCore.Mvc;

namespace IGS.Web.Controllers
{
    public class CommonController : Controller
    {
        [HttpGet]
        public IActionResult LoadExperienceSection(string? categoryIds, string? pageIds, string? orderBy, bool? isAdmin = false, int pageNumber = 1, int itemsPerPage = 8)
        {
            return ViewComponent("ExperienceSection", new { categoryIds, pageIds, orderBy, isAdmin, pageNumber, itemsPerPage });
        }

        [HttpGet]
        public IActionResult LoadInsightSection(string? categoryIds, string? pageIds, string? orderBy)
        {
            return ViewComponent("InsightSection", new { categoryIds, pageIds, orderBy });
        }

        [HttpGet]
        public IActionResult LoadTeamSection(string? categoryIds = null, string? locationIds = null, string? orderBy = "DisplayOrder", bool? isAdmin = false)
        {
            return ViewComponent("TeamSection", new { categoryIds, locationIds, orderBy, isAdmin });
        }
    }
}
