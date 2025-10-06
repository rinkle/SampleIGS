using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models;
using IGS.Models.ViewModels;
using IGS.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IGS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = (UserRoles.Admin + "," + UserRoles.SuperAdmin))]
    [RemoveCache]
    public class NewsController : BaseController
    {
        private readonly INewsVmService _newsVmService;
        private readonly INewsService _newsService;
        private readonly ILoggerService _logger;
        private readonly string _baseUrl;
        private readonly IUnitOfWork _unitOfWork;
        private readonly GlobalEnvironmentSetting _globalEnvironment;

        public NewsController(
            IOptions<AppSettings> options,
            ILoggerService logger,
            INewsVmService newsVmService,
            INewsService newsService,
            IUnitOfWork unitOfWork,
            GlobalEnvironmentSetting globalEnvironment)
        {
            _baseUrl = options.Value.BaseUrl;
            _logger = logger;
            _newsVmService = newsVmService;
            _newsService = newsService;
            _unitOfWork = unitOfWork;
            _globalEnvironment = globalEnvironment;
        }

        // GET: /Admin/News
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = await _newsVmService.GetNewsVmAsync(null, null, null, isAdmin: false);
            return View(vm);
        }

        #region Manage News
        // GET: /Admin/News/ManageNews/5
        [HttpGet]
        public async Task<IActionResult> ManageNews(string id)
        {
            if (!int.TryParse(id, out var newsId))
                return BadRequest("Invalid News Id");

            ViewBag.NewsId = newsId;
            var vm = await _newsVmService.GetNewsModelAsync(newsId);

            vm.NewsInfo.NewsType = vm.NewsInfo.NewsType ?? 1;
            return View(vm);
        }

        // POST: /Admin/News/SaveNewsData
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNewsData(NewsModel model)
        {
            if (model == null || model.NewsInfo == null)
                return BadRequest("Invalid data submitted");

            try
            {
                await _newsService.SaveNewsAsync(model);

                SuccessNotification("News saved successfully.");
                return Redirect(_baseUrl + "admin/news/");
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error saving News");
                ErrorNotification("Error while saving News: " + ex.Message);
                return Redirect(_baseUrl + "admin/news/");
            }
        }
        #endregion

        #region Delete Logo
        [HttpPost]
        public async Task<IActionResult> DeleteLogo(int id)
        {
            bool success = await _newsService.DeleteLogoAsync(id, n => n.Logo = null);
            return Json(new { isSuccess = success, message = success ? Message.DeleteSuccessMessage : Message.DataNotFoundMessage });
        }
        #endregion

        #region Delete News
        [HttpPost]
        public async Task<IActionResult> DeleteNews(int id)
        {
            bool status = false;
            string returnMessage;

            try
            {
                status = await _newsService.DeleteNewsAsync(id);
                returnMessage = status ? Message.DeleteSuccessMessage : Message.DataNotFoundMessage;
            }
            catch
            {
                returnMessage = "An error occurred while deleting the news.";
            }

            return Json(new { isSuccess = status, message = returnMessage });
        }
        #endregion


        #region save insight common data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNewsCommonData(NewsViewModel model)
        {
            if (model == null || model.NewsCommonData == null)
                return BadRequest("Invalid data submitted");

            try
            {
                // ✅ Pass the entity, not the keyless SP result
                await _newsService.SaveNewsCommonDataAsync(model.NewsCommonData);

                SuccessNotification("News common data updated successfully.");
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error saving NewsCommonData");
                ErrorNotification("Error while saving News common data: " + ex.Message);
            }

            return Redirect(_baseUrl + "admin/news/");
        }


        #endregion
    }
}
