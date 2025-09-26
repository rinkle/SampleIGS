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

        // ✅ New methods (for ExperienceModel)
        Task<GetExperienceDetail_Result?> GetExperienceDetailByIdAsync(int experienceId, string? industryCategoryIds = null, string? pageIds = null, string? orderBy = null);
        Task<IEnumerable<GetExperienceIndustryCategoryMapping_Result>> GetExperienceIndustryCategoryMappingAsync(int experienceId);
        Task<IEnumerable<GetExperiencePageMapping_Result>> GetExperiencePageMappingAsync(int experienceId);


    }
}
