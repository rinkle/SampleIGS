using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models.KeyLessModels;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Implementations
{
    public class TransactionServicesService : ITransactionServicesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly GlobalEnvironmentSetting _globalEnvironment;
        private readonly ICommonListingService _commonListingService;


        public TransactionServicesService(IUnitOfWork unitOfWork, ILoggerService logger, ICommonListingService commonListingService, GlobalEnvironmentSetting globalEnvironment)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _commonListingService = commonListingService;
            _globalEnvironment = globalEnvironment;
        }

        public async Task SaveTransactionServiceAsync(TransactionServicesViewModel model)
        {
            try
            {
                if (model == null) return;

                var existing = await _unitOfWork.TransactionService.GetAsync(h => h.Id == model.TransactionService.Id, tracked: true);
                if (existing == null) return;

                // ✅ Save related listings
                if (model.CoreAreasofFocus?.Any() == true)
                {
                    await _commonListingService.SaveCommonListingAsync(model.CoreAreasofFocus);
                }
                // Map fields
                existing.AreasofFocusHeading = model.TransactionService.AreasofFocusHeading;
                existing.AreasofFocusDescription = model.TransactionService.AreasofFocusDescription;
                existing.IndustryExpertiseHeading = model.TransactionService.IndustryExpertiseHeading;
                existing.IndustryExpertiseSubHeading = model.TransactionService.IndustryExpertiseSubHeading;
                existing.IndustryExpertiseDescription = model.TransactionService.IndustryExpertiseDescription;
                existing.RecentProjectHeading = model.TransactionService.RecentProjectHeading;
                existing.RecentProjectDescription = model.TransactionService.RecentProjectDescription;
                existing.InsightHeading = model.TransactionService.InsightHeading;
                // Audit
                existing.ModifiedDate = DateTime.Now;
                existing.ModifiedBy = _globalEnvironment.UserId;

                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in SaveTransactionServiceAsync");
            }
        }
    }
}
