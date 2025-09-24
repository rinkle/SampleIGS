using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Interfaces
{
    public interface ITransactionServicesVmService
    {
        Task<TransactionServicesViewModel> GetTransactionServicesVmAsync(bool isAdmin = false);
    }
}
