using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;
using IGS.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Buffers.Text;
using System.Security.Claims;

namespace IGS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = (UserRoles.Admin + "," + UserRoles.SuperAdmin))]
    [RemoveCache]
    public class TransactionServicesController : BaseController
    {
        private readonly string baseUrl;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly ICommonListingService _commonListingService;
        public TransactionServicesController(IOptions<AppSettings> options, IUnitOfWork unitOfWork, ILoggerService logger, ICommonListingService commonListingService)
        {
            baseUrl = options.Value.BaseUrl;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _commonListingService = commonListingService;
        }
        public async Task<IActionResult> Index()
        {
            var transactionServiceData = await _unitOfWork.TransactionService.GetAsync(h => h.Id == 1, tracked: true);
            try
            {
                var transactionServiceResult = await _unitOfWork.TransactionService.GetTransactionServiceFromSpAsync();
                var allListings = await _unitOfWork.CommonListing.GetCommonListingFromSpAsync((int)PageEnum.TransactionServices);
                var vm = new TransactionServicesViewModel(transactionServiceResult, allListings.ToList(), true);
                return View(vm);
            }
            catch (Exception Ex)
            {
                int errorId = await _logger.LogErrorAsync(Ex, "Error in TransactionServices/Index");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
            }
            return View(null);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveTransactionServiceData(TransactionServicesViewModel model, IFormFile? Brochure)
        {
            try
            {
                if (model.TransactionService != null)
                {
                    await _commonListingService.SaveCommonListingAsync(model.CoreAreasofFocus);
                    var transactionServiceData = await _unitOfWork.TransactionService.GetAsync(h => h.Id == model.TransactionService.Id, tracked: true);
                    if (transactionServiceData != null)
                    {
                        transactionServiceData.AreasofFocusHeading = model.TransactionService.AreasofFocusHeading;
                        transactionServiceData.AreasofFocusDescription = model.TransactionService.AreasofFocusDescription;
                        transactionServiceData.IndustryExpertiseHeading = model.TransactionService.IndustryExpertiseHeading;
                        transactionServiceData.IndustryExpertiseSubHeading = model.TransactionService.IndustryExpertiseSubHeading;
                        transactionServiceData.IndustryExpertiseDescription = model.TransactionService.IndustryExpertiseDescription;
                        transactionServiceData.RecentProjectHeading = model.TransactionService.RecentProjectHeading;
                        transactionServiceData.RecentProjectDescription = model.TransactionService.RecentProjectDescription;
                        transactionServiceData.InsightHeading = model.TransactionService.InsightHeading;
                        transactionServiceData.InsightHeading = model.TransactionService.InsightHeading;
                        transactionServiceData.ModifiedDate = DateTime.Now;
                        transactionServiceData.ModifiedBy = User?.Identity is ClaimsIdentity identity
                            ? identity.FindFirst(ClaimTypes.NameIdentifier)?.Value
                            : null;
                        await _unitOfWork.SaveAsync();
                        SuccessNotification(Message.SuccessMessage);
                        return Redirect(baseUrl + "admin/transactionservices/");
                    }
                }
            }
            catch (Exception Ex)
            {
                int errorId = await _logger.LogErrorAsync(Ex, "Error in TransactionServices/SaveTransactionServicesData");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
            }

            return View("Index", model);
        }


    }

}
