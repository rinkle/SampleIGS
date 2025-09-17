using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services;
using IGS.Models;
using Microsoft.AspNetCore.Mvc;

namespace IGS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommonController : Controller
    {
        private readonly GlobalCookies _cookies;
        private readonly GlobalEnvironmentSetting _globalEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly ICommonListingService _commonListingService;

        public CommonController(
            GlobalCookies cookies,
            GlobalEnvironmentSetting globalEnvironment,
            IUnitOfWork unitOfWork,
            ILoggerService logger,
            ICommonListingService commonListingService)
        {
            _cookies = cookies;
            _globalEnvironment = globalEnvironment;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _commonListingService = commonListingService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Menu Settings
        [HttpPost]
        public JsonResult UpdateSetting(string currentView)
        {
            string newValue = currentView == "Desktop" ? "Mobile" : "Desktop";
            _cookies.CheckDesktop = newValue;

            return Json(new { currentView = newValue });
        }
        #endregion

        #region Delete Common Listing

        [HttpPost]
        [AjaxOnly] // ✅ Custom filter ensures AJAX-only access
        public async Task<JsonResult> DeleteCommonListing([FromBody] int id)
        {
            bool status = false;
            string returnMessage = string.Empty;

            try
            {
                var commonListDetails = await _unitOfWork.CommonListing.GetAsync(c => c.Id == id, tracked: true);
                if (commonListDetails != null)
                {
                    commonListDetails.IsActive = false;
                    commonListDetails.ModifiedBy = _globalEnvironment.UserId;
                    commonListDetails.ModifiedDate = DateTime.Now;

                    await _unitOfWork.SaveAsync();

                    status = true;
                    returnMessage = Message.DeleteSuccessMessage;
                }
                else
                {
                    returnMessage = Message.DataNotFoundMessage;
                }
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in DeleteCommonListing");
                returnMessage = "Something went wrong.";
            }

            return Json(new { IsSuccess = status, Message = returnMessage });
        }

        [HttpPost]
        [AjaxOnly]
        public async Task<JsonResult> DeleteCommonImageListing([FromBody] int id)
        {
            var response = new
            {
                IsSuccess = false,
                Message = "An unexpected error occurred." // default fallback
            };

            try
            {
                var commonListDetails = await _unitOfWork.CommonListing.GetAsync(c => c.Id == id, tracked: true);
                if (commonListDetails != null)
                {
                    commonListDetails.UploadedData = null;
                    commonListDetails.ModifiedBy = _globalEnvironment.UserId;
                    commonListDetails.ModifiedDate = DateTime.Now;

                    await _unitOfWork.SaveAsync();

                    response = new
                    {
                        IsSuccess = true,
                        Message = Message.DeleteSuccessMessage
                    };
                }
                else
                {
                    response = new
                    {
                        IsSuccess = false,
                        Message = Message.DataNotFoundMessage
                    };
                }
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in DeleteCommonImageListing");
                response = new
                {
                    IsSuccess = false,
                    Message = "An error occurred while deleting the image."
                };
            }

            return Json(response);
        }


        #endregion
    }
}
