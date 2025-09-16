using IGS.Dal.Data;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Sql;

namespace IGS.Dal.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private readonly ISqlHelper _sql;

        public IHomeRepository Home { get; private set; }

        public UnitOfWork(ApplicationDbContext db, ISqlHelper sql)
        {
            _db = db;
            _sql = sql;
            Home = new HomeRepository(_db, _sql);
        }

        public void Save() => _db.SaveChanges();

        public async Task SaveAsync() => await _db.SaveChangesAsync();
    }
}
