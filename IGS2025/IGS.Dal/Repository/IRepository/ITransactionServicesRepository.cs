using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository.IRepository
{
    public interface ITransactionServiceRepository : IRepository<TransactionService>
    {
        void Update(TransactionService obj);
        Task<GetTransactionService_Result?> GetTransactionServiceFromSpAsync();
    }
}
