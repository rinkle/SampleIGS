using Globalsetting;
using IGS.Dal.Services.Implementations;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;
using IGS.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IGS.Web.Controllers
{
    public class TeamController : BaseController
    {
        private readonly string _baseUrl;
        private readonly ILoggerService _logger;

        public TeamController(IOptions<AppSettings> options, ILoggerService logger)
        {
            _baseUrl = options.Value.BaseUrl;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
