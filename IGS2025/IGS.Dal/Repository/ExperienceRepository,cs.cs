using IGS.Dal.Data;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Repository.Repository;
using IGS.Dal.Sql;
using IGS.Models;
using IGS.Models.KeyLessModels;
using Microsoft.EntityFrameworkCore;

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

        public async Task<GetExperienceDetail_Result?> GetExperienceDetailByIdAsync(
            int experienceId,
            string? industryCategoryIds = null,
            string? pageIds = null,
            string? orderBy = null)
        {
            var result = await _sql.QueryAsync<GetExperienceDetail_Result>(
                "dbo.GetExperienceDetail",
                new
                {
                    prmExperienceId = experienceId,
                    prmIndustriesCategoryIds = industryCategoryIds,
                    prmPageIds = pageIds,
                    prmOrderBy = orderBy
                },
                isStoredProc: true);

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<GetExperienceIndustryCategoryMapping_Result>> GetExperienceIndustryCategoryMappingAsync(int experienceId)
        {
            return await _sql.QueryAsync<GetExperienceIndustryCategoryMapping_Result>(
                "dbo.GetExperienceIndustryCategoryMapping",
                new { ExperienceId = experienceId },
                isStoredProc: true);
        }

        public async Task<IEnumerable<GetExperiencePageMapping_Result>> GetExperiencePageMappingAsync(int experienceId)
        {
            return await _sql.QueryAsync<GetExperiencePageMapping_Result>(
                "dbo.GetExperiencePageMapping",
                new { ExperienceId = experienceId },
                isStoredProc: true);
        }

        // ✅ Replace mappings
        public async Task ReplaceIndustryMappingsAsync(int experienceId, IEnumerable<int> categoryIds)
        {
            var old = _db.ExperienceIndustryMappings.Where(x => x.Fk_ExperienceId == experienceId);
            _db.ExperienceIndustryMappings.RemoveRange(old);

            foreach (var id in categoryIds)
            {
                await _db.ExperienceIndustryMappings.AddAsync(new ExperienceIndustryMapping
                {
                    Fk_ExperienceId = experienceId,
                    Fk_IndustryCategoryId = id,
                });
            }

            await _db.SaveChangesAsync();
        }

        public async Task ReplacePageMappingsAsync(int experienceId, IEnumerable<int> pageIds)
        {
            var old = _db.ExperiencePageMappings.Where(x => x.FK_ExperienceId == experienceId);
            _db.ExperiencePageMappings.RemoveRange(old);

            foreach (var id in pageIds)
            {
                await _db.ExperiencePageMappings.AddAsync(new ExperiencePageMapping
                {
                    FK_ExperienceId = experienceId,
                    FK_PageId = id,
                    //CreatedDate = DateTime.Now
                });
            }

            await _db.SaveChangesAsync();
        }
        public async Task UpdateExperienceUrlAsync(int experienceId)
        {
            await _sql.ExecuteAsync(
                "dbo.UpdateExperienceUrl",
                new { ExperienceId = experienceId },
                isStoredProc: true
            );
        }
    }
}
