using IGS.Dal.Data;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Repository.Repository;
using IGS.Dal.Sql;
using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository
{
    public class TeamTitleRepository : Repository<TeamTitle>, ITeamTitleRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ISqlHelper _sql;

        public TeamTitleRepository(ApplicationDbContext db, ISqlHelper sql) : base(db)
        {
            _db = db;
            _sql = sql;
        }

        public async Task<IEnumerable<GetTeamTitle_Result>> GetTeamTitleListAsync()
        {
            return await _sql.QueryAsync<GetTeamTitle_Result>(
                "dbo.GetTeamTitle",
                isStoredProc: true
            );
        }

        public async Task<GetTeamTitle_Result?> GetTeamTitleDetailByIdAsync(int id)
        {
            var result = await _sql.QueryAsync<GetTeamTitle_Result>(
                "dbo.GetTeamTitleById",
                new { prmId = id },
                isStoredProc: true
            );

            return result.FirstOrDefault();
        }

        public async Task UpdateTeamTitleUrlAsync(int id)
        {
            await _sql.ExecuteAsync(
                "dbo.UpdateTeamTitleUrl",
                new { prmId = id },
                isStoredProc: true
            );
        }
    }
}
