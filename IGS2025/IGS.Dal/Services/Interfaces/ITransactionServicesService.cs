using IGS.Models.KeyLessModels;

namespace IGS.Dal.Services.Interfaces
{
    public interface ITransactionServicesService
    {
        Task SaveTransactionServiceAsync(GetTransactionService_Result transactionService, string? userId);
    }
}
