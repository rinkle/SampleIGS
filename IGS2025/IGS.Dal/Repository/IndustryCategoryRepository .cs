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
            _sql = sql; // ✅ kept for consistency, even if not used
        }

        public void Update(IndustryCategory obj)
        {
            _db.IndustryCategories.Update(obj);
        }

        public async Task<IEnumerable<GetIndustryCategory_Result>> GetIndustryCategoryFromSpAsync()
        {
            // Using EF Core SP call, _sql not required here
            return await _db.Set<GetIndustryCategory_Result>()
                .FromSqlRaw("EXEC dbo.GetIndustryCategory")
                .ToListAsync();
        }
    }
}
