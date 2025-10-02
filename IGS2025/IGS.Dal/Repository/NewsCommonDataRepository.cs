using IGS.Dal.Data;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Repository.Repository;
using IGS.Models;

namespace IGS.Dal.Repository
{
    public class NewsCommonDataRepository : Repository<NewsCommonData>, INewsCommonDataRepository
    {
        private readonly ApplicationDbContext _db;

        public NewsCommonDataRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
