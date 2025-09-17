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

        [HttpPost]
        public async Task<IActionResult> UploadImages(
    List<IFormFile> files,
    [FromForm] string Filepath,
    [FromForm] int? thumbWidth,
    [FromForm] int? thumbHeight,
    [FromForm] string ExactCropThumbPath,
    [FromForm] string ProportionCropPath,
    [FromForm] string ScaledCropImagePath,
    [FromForm] decimal? ScaledFactor)
        {
            var results = new List<object>();

            try
            {
                // Resolve physical path
                string originalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Filepath);
                if (!Directory.Exists(originalPath))
                    Directory.CreateDirectory(originalPath);

                foreach (var file in files)
                {
                    if (file.Length <= 0) continue;

                    // Save original file
                    string fileName = Path.GetFileName(file.FileName);
                    string savePath = Path.Combine(originalPath, fileName);

                    // If file already exists, add random suffix
                    if (System.IO.File.Exists(savePath))
                    {
                        string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                        string ext = Path.GetExtension(fileName);
                        fileName = $"{nameWithoutExt}_{Guid.NewGuid():N}{ext}";
                        savePath = Path.Combine(originalPath, fileName);
                    }

                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Optional: Generate thumbnails (stubbed — you can wire up your ImageCropper here)
                    if (!string.IsNullOrEmpty(ExactCropThumbPath) && (thumbWidth > 0 || thumbHeight > 0))
                    {
                        try
                        {
                            string exactThumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ExactCropThumbPath);
                            string proportionThumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ProportionCropPath);

                            if (!Directory.Exists(exactThumbPath)) Directory.CreateDirectory(exactThumbPath);
                            if (!Directory.Exists(proportionThumbPath)) Directory.CreateDirectory(proportionThumbPath);

                            // Call your ImageCropper service (to be ported to .NET Core)
                            // ImageCropper.ExactSizeImageCrop(savePath, Path.Combine(exactThumbPath, fileName), thumbWidth.Value, thumbHeight.Value);
                            // ImageCropper.CropImageWithNewSize(savePath, Path.Combine(proportionThumbPath, fileName), thumbWidth.Value, thumbHeight.Value);
                        }
                        catch (Exception ex)
                        {
                            await _logger.LogErrorAsync(ex, "Thumbnail generation failed");
                        }
                    }

                    results.Add(new
                    {
                        Name = fileName,
                        Length = file.Length,
                        Type = file.ContentType
                    });
                }

                return Ok(results);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in UploadImages");
                return StatusCode(500, "Image upload failed.");
            }
        }

    }
}
