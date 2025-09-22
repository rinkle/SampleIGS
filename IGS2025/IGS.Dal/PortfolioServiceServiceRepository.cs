using IGS.Dal.Data;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Repository.Repository;
using IGS.Dal.Sql;
using IGS.Models;
using IGS.Models.KeyLessModels;
using Microsoft.EntityFrameworkCore;

namespace IGS.Dal.Repository
{
    public class PortfolioServiceRepository : Repository<PortfolioService>, IPortfolioServiceRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ISqlHelper _sql;

        public PortfolioServiceRepository(ApplicationDbContext db, ISqlHelper sql) : base(db)
        {
            _db = db;
            _sql = sql;
        }

        // Required by IHomeRepository
        public void Update(PortfolioService obj)
        {
            _db.PortfolioServices.Update(obj);
        }
        public async Task<GetPortfolioService_Result?> GetPortfolioServiceFromSpAsync()
        {
            var result = await _sql.QueryAsync<GetPortfolioService_Result>(
                "dbo.GetPortfolioService", isStoredProc: true);

            return result.FirstOrDefault();
        }
    }
}
