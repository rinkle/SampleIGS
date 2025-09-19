using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services;
using IGS.Models;
using IGS.Models.KeyLessModels;
using IGS.Models.ViewModels;
using IGS.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.InteropServices;

namespace IGS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommonController : BaseController
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

        public async Task<IActionResult> PageHeader(string id)
        {
            int pageid = 0;
            Int32.TryParse(id, out pageid);
            ViewBag.hederpageid = pageid;
            var PageInfo = await _unitOfWork.Page.GetAsync(c => c.Id == pageid, tracked: true);
            if (PageInfo==null)
            {
                PageInfo = new Models.Page();
            }

            GetPageHeader_Result pageHeader=new GetPageHeader_Result();
            if (!string.IsNullOrEmpty(PageInfo.Name))
            {
                await _unitOfWork.PageHeader.GetPageHeaderFromSpAsync(PageInfo.Name);
            }
            var allListings = await _unitOfWork.CommonListing.GetCommonListingFromSpAsync((int)PageEnum.Home);
            PageHeaderModel Modal = new PageHeaderModel(pageHeader, allListings.ToList(), true);
            return View(Modal);
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
                    SuccessNotification(returnMessage);
                }
                else
                {
                    returnMessage = Message.DataNotFoundMessage;
                    ErrorNotification(returnMessage);
                }
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in DeleteCommonListing");
                returnMessage = "Something went wrong.";
                SuccessNotification(returnMessage);
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
                    SuccessNotification(response.Message);
                }
                else
                {
                    response = new
                    {
                        IsSuccess = false,
                        Message = Message.DataNotFoundMessage
                    };
                    ErrorNotification(response.Message);
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
                ErrorNotification(response.Message);
            }

            return Json(response);
        }


        #endregion

        #region image upload
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
        #endregion

        #region upload video 
        [HttpPost]
        public async Task<IActionResult> UploadVideoFiles(IFormFile UploadedVideo, [FromForm] string Filepath)
        {
            if (UploadedVideo == null || UploadedVideo.Length == 0)
                return BadRequest("No video file uploaded.");

            try
            {
                // Build save path: wwwroot + Filepath
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Filepath);

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // Get original filename
                string originalFileName = Path.GetFileName(UploadedVideo.FileName);
                string filePath = Path.Combine(uploadsFolder, originalFileName);

                // If file already exists, append random suffix
                if (System.IO.File.Exists(filePath))
                {
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(originalFileName);
                    string extension = Path.GetExtension(originalFileName);
                    string randomSuffix = "_" + Guid.NewGuid().ToString("N").Substring(0, 6);
                    string newFileName = fileNameWithoutExt + randomSuffix + extension;
                    filePath = Path.Combine(uploadsFolder, newFileName);
                    originalFileName = newFileName;
                }

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadedVideo.CopyToAsync(stream);
                }

                // Build result
                var result = new
                {
                    name = originalFileName,
                    type = UploadedVideo.ContentType,
                    size = $"{UploadedVideo.Length} bytes"
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "An error occurred while uploading the video.");
                // log error if you want: await _logger.LogErrorAsync(ex, "Error in UploadVideoFiles");
                return StatusCode(500, "An error occurred while uploading the video.");
            }
        }

        #endregion

        #region upload pdf
        [HttpPost]
        public async Task<IActionResult> UploadPdfFiles(IFormFile UploadedPdf, [FromForm] string Filepath)
        {
            if (UploadedPdf == null || UploadedPdf.Length == 0)
                return BadRequest("No PDF file uploaded.");

            try
            {
                // Save path under wwwroot
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Filepath);
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // Original filename
                string originalFileName = Path.GetFileName(UploadedPdf.FileName);
                string filePath = Path.Combine(uploadsFolder, originalFileName);

                // Prevent overwriting → add random suffix if file exists
                if (System.IO.File.Exists(filePath))
                {
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(originalFileName);
                    string extension = Path.GetExtension(originalFileName);
                    string randomSuffix = "_" + Guid.NewGuid().ToString("N").Substring(0, 6);
                    string newFileName = fileNameWithoutExt + randomSuffix + extension;
                    filePath = Path.Combine(uploadsFolder, newFileName);
                    originalFileName = newFileName;
                }

                // Save PDF
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadedPdf.CopyToAsync(stream);
                }

                // Return JSON
                var result = new
                {
                    name = originalFileName,
                    type = UploadedPdf.ContentType,
                    size = $"{UploadedPdf.Length} bytes"
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "An error occurred while uploading the PDF");
                return StatusCode(500, "An error occurred while uploading the PDF.");
            }
        }
        #endregion
    }
}
