using IGS.Dal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IGS.Web.ViewComponents
{
    public class ExperienceSectionViewComponent : ViewComponent
    {
        private readonly IExperienceVmService _experienceVmService;

        public ExperienceSectionViewComponent(IExperienceVmService experienceVmService)
        {
            _experienceVmService = experienceVmService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? categoryIds = null, string? pageIds = null, string? orderBy = "DisplayOrder")
        {
            // ✅ Use the correct service that returns ExperienceViewModel
            var model = await _experienceVmService.GetExperienceVmAsync(
                industryCategoryIds: categoryIds,
                pageIds: pageIds,
                orderBy: orderBy,
                isAdmin: false
            );

            // Return the partial strongly typed to ExperienceViewModel
            return View("_ExperienceSectionPartial", model);
        }
    }
}
