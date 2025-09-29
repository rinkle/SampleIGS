using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository.IRepository
{
    public interface ITeamRepository : IRepository<Team>
    {
        Task<IEnumerable<GetTeamFilterList_Result>> GetTeamFilterListFromSpAsync(
            string? categoryIds = null,
            string? locationIds = null,
            string? orderBy = null);

        Task<GetTeamDetails_Result?> GetTeamDetailByIdAsync(
            int teamId,
            string? categoryIds = null,
            string? locationIds = null,
            string? orderBy = null);

        Task<IEnumerable<getTeamTeamCategoryMapping_Result>> GetTeamCategoryMappingAsync(int teamId);
        Task<IEnumerable<GetTeamTitle_Result>> GetTeamTitlesAsync();
        Task ReplaceCategoryMappingsAsync(int teamId, IEnumerable<int> categoryIds);
        Task UpdateTeamUrlAsync(int teamId);
    }
}
