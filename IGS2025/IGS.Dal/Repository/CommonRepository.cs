using IGS.Dal.Data;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Repository.Repository;
using IGS.Dal.Sql;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository
{
    public class CommonRepository : Repository<object>, ICommonRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ISqlHelper _sql;

        public CommonRepository(ApplicationDbContext db, ISqlHelper sql) : base(db)
        {
            _db = db;
            _sql = sql;
        }

        public async Task<GetPageHeader_Result?> GetPageHeaderAsync(string pageName)
        {
            var result = await _sql.QueryAsync<GetPageHeader_Result>(
                "dbo.GetPageHeader",
                new { pageName },
                isStoredProc: true);

            return result.FirstOrDefault();
        }

        public async Task<GetOtherContact_Result?> GetOtherContactAsync()
        {
            var result = await _sql.QueryAsync<GetOtherContact_Result>(
                "dbo.GetOtherContact",
                isStoredProc: true);

            return result.FirstOrDefault();
        }
    }
}
