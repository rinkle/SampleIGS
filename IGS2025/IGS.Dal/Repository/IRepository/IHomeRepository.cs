using IGS.Models;
using IGS.Models.ViewModels;

namespace IGS.Dal.Repository.IRepository
{
    public interface IHomeRepository : IRepository<Home>
    {
        void Update(Home obj);
        Task<GetHome_Result?> GetHomeFromSpAsync();
    }
}
