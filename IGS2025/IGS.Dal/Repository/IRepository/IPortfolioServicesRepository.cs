using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository.IRepository
{
    public interface IPortfolioServiceRepository : IRepository<PortfolioService>
    {
        void Update(PortfolioService obj);
        Task<GetPortfolioService_Result?> GetPortfolioServiceFromSpAsync();
    }
}
