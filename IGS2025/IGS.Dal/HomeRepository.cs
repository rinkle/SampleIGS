using IGS.Dal.Repository.IRepository;
using IGS.Dal.Data;
using IGS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using IGS.Dal.Repository.Repository;

namespace IGS.Repository
{
    public class HomeRepository : Repository<Home>, IHomeRepository
    {
        private ApplicationDbContext _db;
        public HomeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Home obj)
        {
            _db.Homes.Update(obj);
        }
    }
}
