using IGS.Dal.Data;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Repository.Repository;
using IGS.Dal.Sql;
using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository
{
    public class ExperienceRepository : Repository<Experience>, IExperienceRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ISqlHelper _sql;

        public ExperienceRepository(ApplicationDbContext db, ISqlHelper sql) : base(db)
        {
            _db = db;
            _sql = sql;
        }

        public async Task<IEnumerable<GetExperienceFilterList_Result>> GetExperienceFilterListFromSpAsync(
            string? industryCategoryIds = null,
            string? pageIds = null,
            string? orderBy = null)
        {
            return await _sql.QueryAsync<GetExperienceFilterList_Result>(
                "dbo.GetExperienceFilterList",
                new
                {
                    prmIndustriesCategoryIds = industryCategoryIds,
                    prmPageIds = pageIds,
                    prmOrderBy = orderBy
                },
                isStoredProc: true);
        }
    }
}
