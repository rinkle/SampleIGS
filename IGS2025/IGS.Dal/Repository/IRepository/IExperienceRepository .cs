using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository.IRepository
{
    public interface IExperienceRepository : IRepository<Experience>
    {
        Task<IEnumerable<GetExperienceFilterList_Result>> GetExperienceFilterListFromSpAsync(
            string? industryCategoryIds = null,
            string? pageIds = null,
            string? orderBy = null
        );

        // ✅ For ExperienceModel
        Task<GetExperienceDetail_Result?> GetExperienceDetailByIdAsync(
            int experienceId,
            string? industryCategoryIds = null,
            string? pageIds = null,
            string? orderBy = null);

        Task<IEnumerable<GetExperienceIndustryCategoryMapping_Result>> GetExperienceIndustryCategoryMappingAsync(int experienceId);
        Task<IEnumerable<GetExperiencePageMapping_Result>> GetExperiencePageMappingAsync(int experienceId);

        // ✅ New replace methods
        Task ReplaceIndustryMappingsAsync(int experienceId, IEnumerable<int> categoryIds);
        Task ReplacePageMappingsAsync(int experienceId, IEnumerable<int> pageIds);
        Task UpdateExperienceUrlAsync(int experienceId);

    }
}
