using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository.IRepository
{
    public interface IExperienceRepository : IRepository<Experience>
    {
        Task<IEnumerable<GetExperienceFilterList_Result>> GetExperienceFilterListFromSpAsync(
            string? industryCategoryIds = null,
            string? pageIds = null,
            string? orderBy = null);

        Task<GetExperienceDetail_Result?> GetExperienceDetailByIdAsync(
            int experienceId,
            string? industryCategoryIds = null,
            string? pageIds = null,
            string? orderBy = null);

        Task<IEnumerable<GetExperienceIndustryCategoryMapping_Result>> GetExperienceIndustryCategoryMappingAsync(int experienceId);
        Task<IEnumerable<GetExperiencePageMapping_Result>> GetExperiencePageMappingAsync(int experienceId);

        // ✅ Write helpers
        Task ReplaceIndustryMappingsAsync(int experienceId, IEnumerable<int> categoryIds);
        Task ReplacePageMappingsAsync(int experienceId, IEnumerable<int> pageIds);
        Task UpdateExperienceUrlAsync(int experienceId);   // <-- here
    }
}
