using IGS.Dal.Data;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Repository.Repository;
using IGS.Dal.Sql;
using IGS.Models;
using IGS.Models.KeyLessModels;
using Microsoft.EntityFrameworkCore;

namespace IGS.Dal.Repository
{
    public class IndustryCategoryRepository : Repository<IndustryCategory>, IIndustryCategoryRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ISqlHelper _sql;

        public IndustryCategoryRepository(ApplicationDbContext db, ISqlHelper sql) : base(db)
        {
            _db = db;
            _sql = sql;
        }

        // Required by IIndustryCategoryRepository
        public void Update(IndustryCategory obj)
        {
            _db.IndustryCategories.Update(obj);
        }
        public async Task<GetIndustryCategory_Result?> GetIndustryCategoryFromSpAsync()
        {
            var result = await _sql.QueryAsync<GetIndustryCategory_Result>(
                "dbo.GetIndustryCategory", isStoredProc: true);

            return result.FirstOrDefault();
        }
    }
}
