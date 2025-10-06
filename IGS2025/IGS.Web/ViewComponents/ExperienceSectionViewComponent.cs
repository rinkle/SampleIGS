using IGS.Dal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IGS.Web.ViewComponents
{
    public class ExperienceSectionViewComponent : ViewComponent
    {
        private readonly IExperienceVmService _experienceVmService;

        public ExperienceSectionViewComponent(IExperienceVmService experienceVmService)
        {
            _experienceVmService = experienceVmService;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            string? categoryIds,
            string? pageIds,
            string? orderBy,
            bool? isAdmin = false,
            int pageNumber = 1,
            int itemsPerPage = 8)
        {
            // Call your VM service with pagination
            var vm = await _experienceVmService.GetExperienceVmAsync(
                categoryIds,
                pageIds,
                orderBy,
                isAdmin ?? false,
                pageNumber,
                itemsPerPage);

            // Return the partial list of experiences
            return View("_ExperienceSectionPartial", vm);
        }
    }
}
