using Globalsetting;
using IGS.Dal.Services.Implementations;
using IGS.Dal.Services.Interfaces;
using IGS.Models;
using IGS.Models.ViewModels;
using IGS.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace IGS.Web.Controllers
{
    public class InsightsController : BaseController
    {
        private readonly string _baseUrl;
        private readonly ILoggerService _logger;
        private readonly INewsVmService _newsVmService;
        private readonly ICommonService _commonService;

        public InsightsController(IOptions<AppSettings> options, ILoggerService logger, INewsVmService newsVmService, ICommonService commonService)
        {
            _baseUrl = options.Value.BaseUrl;
            _logger = logger;
            _newsVmService = newsVmService;
            _commonService = commonService;
        }
        public async Task<IActionResult> Index()
        {
            //ViewBag.pageName = PageName.Insights;
            //ViewBag.canonical = "insights";
            //CommonHeaderFooterModel CommonServiceModel = await _commonService.GetCommonServiceAsync(PageName.Insights);
            //ViewBag.CommonService = CommonServiceModel;
            //var vm = await _newsVmService.GetNewsVmAsync(null, null, null, isAdmin: false);
            //return View(vm);

            try
            {
                ViewBag.pageName = PageName.Insights;
                ViewBag.canonical = "insights";
                CommonHeaderFooterModel CommonServiceModel = await _commonService.GetCommonServiceAsync(PageName.Insights);
                ViewBag.CommonService = CommonServiceModel;
                NewsViewModel vm = await _newsVmService.GetNewsVmAsync(null, null, null, isAdmin: false);
                return View(vm);
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "Error in Home/Index");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
                return View(null);
            }
        }


        [Route("news-article/{newsurl}")]
        [Route("insight-info/{newsurl}")]
        [HttpGet]
        public async Task<IActionResult> InsightInfo(string newsurl)
        {
            //write a methid to get newsid
            int newsId = 0;
            ViewBag.newsurl = newsurl;
            ViewBag.pageName = PageName.Insights;
            ViewBag.canonical = "insight-article/" + newsurl;
            CommonHeaderFooterModel CommonServiceModel = await _commonService.GetCommonServiceAsync(PageName.Experience);
            ViewBag.NewsId = newsId;
            var vm = await _newsVmService.GetNewsModelAsync(newsId);
            vm.NewsInfo.NewsType = vm.NewsInfo.NewsType ?? 1;
            return View(vm);
        }
    }
}
