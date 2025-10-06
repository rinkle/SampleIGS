using IGS.Dal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IGS.Web.ViewComponents
{
    public class InsightSectionViewComponent : ViewComponent
    {
        private readonly INewsVmService _newsVmService;

        public InsightSectionViewComponent(INewsVmService newsVmService)
        {
            _newsVmService = newsVmService;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            string? categoryIds = null,
            string? pageIds = null,
            string? orderBy = "DisplayOrder")
        {
            var model = await _newsVmService.GetNewsVmAsync(
                categoryIds: categoryIds,
                pageIds: pageIds,
                orderBy: orderBy,
                isAdmin: false
            );

            // render partial with strongly typed model
            return View("_InsightSectionPartial", model);
        }
    }
}
