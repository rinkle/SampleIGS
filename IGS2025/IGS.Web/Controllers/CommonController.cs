using Microsoft.AspNetCore.Mvc;

namespace IGS.Web.Controllers
{
    public class CommonController : Controller
    {
        [HttpGet]
        public IActionResult LoadExperienceSection(string? pageIds)
        {
            return ViewComponent("ExperienceSection", new { pageIds });
        }

        [HttpGet]
        public IActionResult LoadInsightSection(string? categoryIds, string? pageIds)
        {
            return ViewComponent("InsightSection", new { categoryIds, pageIds });
        }
    }
}
