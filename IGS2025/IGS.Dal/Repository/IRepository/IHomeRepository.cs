using IGS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGS.Dal.Repository.IRepository
{
    public interface IHomeRepository : IRepository<Home>
    {
        void Update(Home obj);
    }
}
