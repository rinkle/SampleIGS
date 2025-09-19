using IGS.Dal.Data;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Sql;
using IGS.Models;

namespace IGS.Dal.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private readonly ISqlHelper _sql;

        public IHomeRepository Home { get; private set; }
        public ICommonListingRepository CommonListing { get; private set; }
        public IPageHeaderRepository PageHeader { get; private set; }
        public IPageRepository Page { get; private set; }

        public UnitOfWork(ApplicationDbContext db, ISqlHelper sql)
        {
            _db = db;
            _sql = sql;
            Home = new HomeRepository(_db, _sql);
            CommonListing = new CommonListingRepository(_db, _sql);
            PageHeader = new PageHeaderRepository(_db, _sql);
            Page = new PageRepository(_db, _sql);

        }

        public void Save() => _db.SaveChanges();

        public async Task SaveAsync() => await _db.SaveChangesAsync();
    }
}
