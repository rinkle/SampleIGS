using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services;
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
        private readonly ILoggerService _logger;

        public HomeController(IOptions<AppSettings> options, IUnitOfWork unitOfWork, ILoggerService logger)
        {
            baseUrl = options.Value.BaseUrl;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                throw new Exception("Test exception");
                // Call stored procedure via HomeRepository
                var homeResult = await _unitOfWork.Home.GetHomeFromSpAsync();
                // Initialize ViewModel
                var vm = new HomeViewModel(homeResult);
                return View(vm);
            }
            catch (Exception Ex)
            {
                int errorId = await _logger.LogErrorAsync(Ex, "Error in Home/Index");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
            }
            return View(null);

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

            SuccessNotification("Home page data saved successfully!");
            return RedirectToAction(nameof(Index));
        }

    }

}
