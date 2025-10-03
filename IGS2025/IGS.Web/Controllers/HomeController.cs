using Globalsetting;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;
using IGS.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;


namespace IGS.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly string _baseUrl;
        private readonly ILoggerService _logger;
        private readonly IHomeVmService _homeVmService;
        private readonly IHomeService _homeService;

        public HomeController(IOptions<AppSettings> options, ILoggerService logger, IHomeVmService homeVmService, IHomeService homeService)
        {
            _baseUrl = options.Value.BaseUrl;
            _logger = logger;
            _homeVmService = homeVmService;
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                HomeViewModel vm = await _homeVmService.GetHomeVmAsync(isAdmin: true);
                return View(vm);
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "Error in Home/Index");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
                return View(null);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
