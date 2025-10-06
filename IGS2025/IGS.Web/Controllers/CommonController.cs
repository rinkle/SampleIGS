using Microsoft.AspNetCore.Mvc;

namespace IGS.Web.Controllers
{
    public class CommonController : Controller
    {
        [HttpGet]
        public IActionResult LoadExperienceSection(string? categoryIds, string? pageIds,string? orderBy)
        {
            return ViewComponent("ExperienceSection", new { categoryIds, pageIds, orderBy });
        }

        [HttpGet]
        public IActionResult LoadInsightSection(string? categoryIds, string? pageIds, string? orderBy)
        {
            return ViewComponent("InsightSection", new { categoryIds, pageIds, orderBy });
        }
    }
}
