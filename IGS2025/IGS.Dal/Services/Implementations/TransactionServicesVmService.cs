using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Implementations
{
    public class TransactionServicesVmService : ITransactionServicesVmService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;

        public TransactionServicesVmService(IUnitOfWork unitOfWork, ILoggerService logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<TransactionServicesViewModel> GetTransactionServicesVmAsync(bool isAdmin = false)
        {
            try
            {
                var transactionServiceResult = await _unitOfWork.TransactionService.GetTransactionServiceFromSpAsync();
                var allListings = await _unitOfWork.CommonListing.GetCommonListingFromSpAsync((int)PageEnum.TransactionServices);

                return new TransactionServicesViewModel(transactionServiceResult, allListings, isAdmin);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error fetching Transaction Services VM");
                return new TransactionServicesViewModel();
            }
        }
    }
}
