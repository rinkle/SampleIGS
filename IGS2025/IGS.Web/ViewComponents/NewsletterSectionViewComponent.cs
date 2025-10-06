using IGS.Dal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IGS.Web.ViewComponents
{
    public class NewsletterSectionViewComponent : ViewComponent
    {
        private readonly INewsLetterVmService _newsLetterVmService;

        public NewsletterSectionViewComponent(INewsLetterVmService newsLetterVmService)
        {
            _newsLetterVmService = newsLetterVmService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _newsLetterVmService.GetNewsLetterViewModelAsync();
            return View("_NewsletterSectionPartial", model);
        }
    }
}
