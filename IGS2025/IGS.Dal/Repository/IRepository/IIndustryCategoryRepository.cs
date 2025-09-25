using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository.IRepository
{
    public interface IIndustryCategoryRepository : IRepository<IndustryCategory>
    {
        void Update(IndustryCategory obj);
        Task<IEnumerable<GetIndustryCategory_Result>> GetIndustryCategoryFromSpAsync();
    }
}
