using IGS.Dal.Data;
using IGS.Dal.Repository.IRepository;
using IGS.Models;
using IGS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGS.Dal.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IHomeRepository Home { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Home = new HomeRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
