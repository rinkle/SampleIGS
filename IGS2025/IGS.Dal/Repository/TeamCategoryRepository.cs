using IGS.Dal.Data;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Repository.Repository;
using IGS.Dal.Sql;
using IGS.Models;

namespace IGS.Dal.Repository
{
    public class TeamCategoryRepository : Repository<TeamCategory>, ITeamCategoryRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ISqlHelper _sql;

        public TeamCategoryRepository(ApplicationDbContext db, ISqlHelper sql) : base(db)
        {
            _db = db;
            _sql = sql;
        }
    }
}
