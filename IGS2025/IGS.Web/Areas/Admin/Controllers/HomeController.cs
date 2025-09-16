using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Models.ViewModels;
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
    public class HomeController : BaseController
    {
        private readonly string baseUrl;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IOptions<AppSettings> options, IUnitOfWork unitOfWork)
        {
            baseUrl = options.Value.BaseUrl;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            // Call stored procedure via HomeRepository
            var homeResult = await _unitOfWork.Home.GetHomeFromSpAsync();

            // Initialize ViewModel
            var vm = new HomeViewModel(homeResult);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveHomeData(HomeViewModel model)
        {
            if (model.Home != null)
            {
                var entity = await _unitOfWork.Home.GetAsync(h => h.Id == model.Home.Id, tracked: true);
                if (entity != null)
                {
                    entity.Disclaimer = model.Home.Disclaimer;
                    entity.InvestorLogin = model.Home.InvestorLogin;
                    entity.LinkedInUrl = model.Home.LinkedInUrl;
                    entity.VimeoVideoUrl = model.Home.VimeoVideoUrl;

                    await _unitOfWork.SaveAsync();
                }
            }

            SuccessNotification("Home data saved successfully!");
            return RedirectToAction(nameof(Index));
        }

    }

}
