using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Services.Implementations
{
    public class TransactionServicesService : ITransactionServicesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;

        public TransactionServicesService(IUnitOfWork unitOfWork, ILoggerService logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task SaveTransactionServiceAsync(GetTransactionService_Result transactionService, string? userId)
        {
            try
            {
                if (transactionService == null) return;

                var existing = await _unitOfWork.TransactionService.GetAsync(h => h.Id == transactionService.Id, tracked: true);
                if (existing == null) return;

                // Map fields
                existing.AreasofFocusHeading = transactionService.AreasofFocusHeading;
                existing.AreasofFocusDescription = transactionService.AreasofFocusDescription;
                existing.IndustryExpertiseHeading = transactionService.IndustryExpertiseHeading;
                existing.IndustryExpertiseSubHeading = transactionService.IndustryExpertiseSubHeading;
                existing.IndustryExpertiseDescription = transactionService.IndustryExpertiseDescription;
                existing.RecentProjectHeading = transactionService.RecentProjectHeading;
                existing.RecentProjectDescription = transactionService.RecentProjectDescription;
                existing.InsightHeading = transactionService.InsightHeading;

                // Audit
                existing.ModifiedDate = DateTime.Now;
                existing.ModifiedBy = userId;

                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in SaveTransactionServiceAsync");
            }
        }
    }
}
