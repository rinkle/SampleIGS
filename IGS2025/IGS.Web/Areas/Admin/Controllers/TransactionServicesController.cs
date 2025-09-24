using Globalsetting;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;
using IGS.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace IGS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = (UserRoles.Admin + "," + UserRoles.SuperAdmin))]
    [RemoveCache]
    public class TransactionServicesController : BaseController
    {
        private readonly string _baseUrl;
        private readonly ILoggerService _logger;
        private readonly ITransactionServicesVmService _transactionVmService;
        private readonly ITransactionServicesService _transactionService;
        private readonly ICommonListingService _commonListingService;

        public TransactionServicesController(
            IOptions<AppSettings> options,
            ILoggerService logger,
            ITransactionServicesVmService transactionVmService,
            ITransactionServicesService transactionService,
            ICommonListingService commonListingService)
        {
            _baseUrl = options.Value.BaseUrl;
            _logger = logger;
            _transactionVmService = transactionVmService;
            _transactionService = transactionService;
            _commonListingService = commonListingService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var vm = await _transactionVmService.GetTransactionServicesVmAsync(isAdmin: true);
                return View(vm);
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "Error in TransactionServices/Index");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
                return View(null);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveTransactionServiceData(TransactionServicesViewModel model)
        {
            try
            {
                if (model.TransactionService != null)
                {
                    // ✅ Save related listings
                    if (model.CoreAreasofFocus?.Any() == true)
                    {
                        await _commonListingService.SaveCommonListingAsync(model.CoreAreasofFocus);
                    }

                    // ✅ Save main entity
                    var userId = User?.Identity is ClaimsIdentity identity
                        ? identity.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        : null;

                    await _transactionService.SaveTransactionServiceAsync(model);

                    SuccessNotification("Transaction Services data saved successfully!");
                    return Redirect(_baseUrl + "admin/transactionservices/");
                }
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "Error in TransactionServices/SaveTransactionServiceData");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
            }

            return View("Index", model);
        }
    }
}
