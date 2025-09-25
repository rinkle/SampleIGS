using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository.IRepository
{
    public interface IExperienceRepository : IRepository<Experience>
    {
        Task<IEnumerable<GetExperienceFilterList_Result>> GetExperienceFilterListFromSpAsync(
            string? industryCategoryIds = null, // e.g. "1,2"
            string? pageIds = null,             // e.g. "3,5"
            string? orderBy = null              // "ClientName" | "TransactionDate" | null
        );
    }
}
