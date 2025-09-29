using IGS.Dal.Data;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Repository.Repository;
using IGS.Dal.Sql;
using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository
{
    public class TeamRepository : Repository<Team>, ITeamRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ISqlHelper _sql;

        public TeamRepository(ApplicationDbContext db, ISqlHelper sql) : base(db)
        {
            _db = db;
            _sql = sql;
        }

        public async Task<IEnumerable<GetTeamFilterList_Result>> GetTeamFilterListFromSpAsync(
            string? categoryIds = null,
            string? locationIds = null,
            string? orderBy = null)
        {
            return await _sql.QueryAsync<GetTeamFilterList_Result>(
                "dbo.GetTeamFilterList",
                new
                {
                    prmCategoryIds = categoryIds,
                    prmLocationIds = locationIds,
                    prmOrderBy = orderBy
                },
                isStoredProc: true);
        }

        public async Task<GetTeamDetails_Result?> GetTeamDetailByIdAsync(
            int teamId,
            string? categoryIds = null,
            string? locationIds = null,
            string? orderBy = null)
        {
            var result = await _sql.QueryAsync<GetTeamDetails_Result>(
                "dbo.GetTeamDetails", 
                new
                {
                    prmTeamId = teamId,
                    prmCategoryIds = categoryIds,
                    prmLocationIds = locationIds,
                    prmOrderBy = orderBy
                },
                isStoredProc: true);

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<getTeamTeamCategoryMapping_Result>> GetTeamCategoryMappingAsync(int teamId)
        {
            return await _sql.QueryAsync<getTeamTeamCategoryMapping_Result>(
                "dbo.getTeamTeamCategoryMapping",
                new { TeamId = teamId },
                isStoredProc: true);
        }

        public async Task UpdateTeamUrlAsync(int teamId)
        {
            await _sql.ExecuteAsync(
                "dbo.UpdateTeamUrl",
                new { TeamId = teamId },
                isStoredProc: true
            );
        }

        public async Task<IEnumerable<GetTeamTitle_Result>> GetTeamTitlesAsync()
        {
            return await _sql.QueryAsync<GetTeamTitle_Result>(
                "dbo.GetTeamTitle",  
                null,
                isStoredProc: true
            );
        }

    }
}
