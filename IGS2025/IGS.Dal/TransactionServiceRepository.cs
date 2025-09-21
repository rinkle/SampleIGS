using IGS.Dal.Data;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Repository.Repository;
using IGS.Dal.Sql;
using IGS.Models;
using IGS.Models.KeyLessModels;
using Microsoft.EntityFrameworkCore;

namespace IGS.Dal.Repository
{
    public class TransactionServiceRepository : Repository<TransactionService>, ITransactionServiceRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ISqlHelper _sql;

        public TransactionServiceRepository(ApplicationDbContext db, ISqlHelper sql) : base(db)
        {
            _db = db;
            _sql = sql;
        }

        // Required by IHomeRepository
        public void Update(TransactionService obj)
        {
            _db.TransactionServices.Update(obj);
        }
        public async Task<GetTransactionService_Result?> GetTransactionServiceFromSpAsync()
        {
            var result = await _sql.QueryAsync<GetTransactionService_Result>(
                "dbo.GetTransactionService", isStoredProc: true);

            return result.FirstOrDefault();
        }
    }
}
