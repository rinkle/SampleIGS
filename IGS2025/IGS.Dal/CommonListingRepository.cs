using IGS.Dal.Data;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Repository.Repository;
using IGS.Dal.Sql;
using IGS.Models;
using IGS.Models.KeyLessModels;
using Microsoft.Data.SqlClient;

namespace IGS.Dal.Repository
{
    public class CommonListingRepository : Repository<CommonListing>, ICommonListingRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ISqlHelper _sql;
        public CommonListingRepository(ApplicationDbContext db, ISqlHelper sql) : base(db)
        {
            _db = db;
            _sql = sql;
        }

        public void Update(CommonListing entity)
        {
            _db.CommonListings.Update(entity);
        }
        public async Task<IEnumerable<GetCommonListing_Result>> GetCommonListingFromSpAsync(int pageId)
        {
            var parameters = new[]
            {
                new SqlParameter("@PageId", pageId)
            };

            return await _sql.QueryAsync<GetCommonListing_Result>(
                "dbo.GetCommonListing",
                parameters,
                isStoredProc: true
            );
        }
    }
}
