using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository.IRepository
{
    public interface IIndustryRepository : IRepository<Industry>
    {
        void Update(Industry obj);

        // Stored procedure result
        Task<GetIndustry_Result?> GetIndustryFromSpAsync();

        Task<IEnumerable<GetIndustryCategory_Result>> GetIndustryCategoryFromSpAsync();
    }
}
