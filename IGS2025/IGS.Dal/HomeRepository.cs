using IGS.Dal.Data;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Repository.Repository;
using IGS.Dal.Sql;
using IGS.Models;
using IGS.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace IGS.Dal.Repository
{
    public class HomeRepository : Repository<Home>, IHomeRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ISqlHelper _sql;

        public HomeRepository(ApplicationDbContext db, ISqlHelper sql) : base(db)
        {
            _db = db;
            _sql = sql;
        }

        // Required by IHomeRepository
        public void Update(Home obj)
        {
            _db.Homes.Update(obj);
        }
        public async Task<GetHome_Result?> GetHomeFromSpAsync()
        {
            var result = await _sql.QueryAsync<GetHome_Result>(
                "dbo.GetHome", isStoredProc: true);

            return result.FirstOrDefault();
        }
    }
}
