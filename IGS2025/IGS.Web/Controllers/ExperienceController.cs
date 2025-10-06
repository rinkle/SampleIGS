using Globalsetting;
using IGS.Dal.Services.Implementations;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;
using IGS.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;


namespace IGS.Web.Controllers
{
    public class ExperienceController : BaseController
    {
        private readonly string _baseUrl;
        private readonly ILoggerService _logger;
        private readonly IExperienceVmService _experienceVmService;
        private readonly ICommonService _commonService;


        public ExperienceController(IOptions<AppSettings> options, ILoggerService logger, IExperienceVmService experienceVmService, ICommonService commonService)
        {
            _baseUrl = options.Value.BaseUrl;
            _logger = logger;
            _experienceVmService = experienceVmService;
            _commonService = commonService;
        }

        [Route("experience")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.pageName = PageName.Experience;
            ViewBag.canonical = "industries";
            CommonHeaderFooterModel CommonServiceModel = await _commonService.GetCommonServiceAsync(PageName.Experience);
            ViewBag.CommonService = CommonServiceModel;
            if (CommonServiceModel!=null && CommonServiceModel.HeaderInfo!=null)
            {
                ViewBag.HeaderAdditionalInfo = CommonServiceModel?.HeaderInfo?.Additionalinfo;
            }
            ExperienceViewModel vm = await _experienceVmService.GetExperienceVmAsync("-1", "-1", null, isAdmin: false);
            return View(vm);
        }
    }
}
