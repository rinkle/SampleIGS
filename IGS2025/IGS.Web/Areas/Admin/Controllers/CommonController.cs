using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services;
using IGS.Models;
using IGS.Models.KeyLessModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IGS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommonController : Controller
    {
        private readonly GlobalCookies _cookies;
        private readonly GlobalEnvironmentSetting _globalEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;

        public CommonController(GlobalCookies cookies, GlobalEnvironmentSetting globalEnvironment, IUnitOfWork unitOfWork, ILoggerService logger)
        {
            _cookies = cookies;
            _globalEnvironment = globalEnvironment;
            _unitOfWork = unitOfWork;
            _logger = logger;
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

        #region Save Common Settings
        public async Task SaveCommonListingAsync(List<GetCommonListing_Result> commonListItems)
        {
            try
            {
                if (commonListItems != null && commonListItems.Count > 0)
                {
                    foreach (var item in commonListItems)
                    {
                        var commonListItem = await _unitOfWork.CommonListing.GetAsync(
                            c => c.Id == item.Id,
                            tracked: true);

                        if (commonListItem == null)
                        {
                            commonListItem = new CommonListing
                            {
                                CreatedBy = _globalEnvironment.UserId,
                                CreatedDate = DateTime.Now,
                                IsActive = true
                            };
                            await _unitOfWork.CommonListing.AddAsync(commonListItem);
                        }

                        // Map fields
                        commonListItem.Fk_PageId = item.Fk_PageId;
                        commonListItem.Section = item.Section;
                        commonListItem.Heading = item.Heading;
                        commonListItem.SubHeading = item.SubHeading;
                        commonListItem.Description = item.Description;
                        commonListItem.UploadedData = item.UploadedData;
                        commonListItem.AdditionalImage1 = item.AdditionalImage1;
                        commonListItem.AdditionalImage2 = item.AdditionalImage2;
                        commonListItem.AdditionalImage3 = item.AdditionalImage3;
                        commonListItem.ImageLabel = item.ImageLabel;
                        commonListItem.AdditionalSubHeading = item.AdditionalSubHeading;
                        commonListItem.ReferanceUrl = item.ReferanceUrl;
                        commonListItem.DisplayOrder = item.DisplayOrder;

                        if (item.Fk_CaseStudyId.HasValue)
                            commonListItem.Fk_CaseStudyId = item.Fk_CaseStudyId.Value;

                        if (item.Fk_PortId.HasValue)
                            commonListItem.Fk_PortId = item.Fk_PortId.Value;

                        // Update audit fields
                        commonListItem.ModifiedBy = _globalEnvironment.UserId;
                        commonListItem.ModifiedDate = DateTime.Now;
                    }

                    await _unitOfWork.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in SaveCommonListingAsync");
            }
        }
        #endregion

        #region Delete Common Listing

        [HttpPost]
        [AjaxOnly] // ✅ Custom filter ensures AJAX-only access
        public async Task<JsonResult> DeleteCommonListing(int id)
        {
            bool status = false;
            string returnMessage;

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
                returnMessage = "An error occurred while deleting.";
            }

            return Json(new { IsSuccess = status, Message = returnMessage });
        }

        [HttpPost]
        [AjaxOnly] // ✅ Custom filter ensures AJAX-only access
        public async Task<JsonResult> DeleteCommonImageListing(int id)
        {
            bool status = false;
            string returnMessage;

            try
            {
                var commonListDetails = await _unitOfWork.CommonListing.GetAsync(c => c.Id == id, tracked: true);
                if (commonListDetails != null)
                {
                    commonListDetails.UploadedData = null;
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
                await _logger.LogErrorAsync(ex, "Error in DeleteCommonImageListing");
                returnMessage = "An error occurred while deleting image.";
            }

            return Json(new { IsSuccess = status, Message = returnMessage });
        }

        #endregion
    }
}
