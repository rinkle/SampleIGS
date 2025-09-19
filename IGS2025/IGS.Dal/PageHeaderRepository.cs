using IGS.Dal.Data;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Repository.Repository;
using IGS.Dal.Sql;
using IGS.Models;
using IGS.Models.KeyLessModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace IGS.Dal.Repository
{
    public class PageHeaderRepository : Repository<PageHeader>, IPageHeaderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ISqlHelper _sql;

        public PageHeaderRepository(ApplicationDbContext db, ISqlHelper sql) : base(db)
        {
            _db = db;
            _sql = sql;
        }

        // Required by IPageHeaderRepository
        public void Update(PageHeader obj)
        {
            _db.PageHeaders.Update(obj);
        }

        public async Task<GetPageHeader_Result?> GetPageHeaderFromSpAsync(string pageName)
        {
            var parameters = new[]
            {
                new SqlParameter("@pageName", pageName)
            };
            var result = await _sql.QueryAsync<GetPageHeader_Result>(
                "dbo.GetPageHeader", parameters,isStoredProc: true);
            return result.FirstOrDefault();
        }
    }
}
