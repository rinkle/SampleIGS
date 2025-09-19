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
    public class PageRepository : Repository<Page>, IPageRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ISqlHelper _sql;

        public PageRepository(ApplicationDbContext db, ISqlHelper sql) : base(db)
        {
            _db = db;
            _sql = sql;
        }

        // Required by IPageRepository
        public void Update(Page obj)
        {
            _db.Pages.Update(obj);
        }
    }
}
