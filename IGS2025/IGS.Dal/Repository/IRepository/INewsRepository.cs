using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository.IRepository
{
    public interface INewsRepository : IRepository<News>
    {
        Task<IEnumerable<GetNewsFilterList_Result>> GetNewsFilterListAsync(
            string? categoryIds = null,
            string? pageIds = null,
            string? orderBy = null);

        Task<GetNewsDetail_Result?> GetNewsDetailByIdAsync(
            int newsId,
            string? categoryIds = null,
            string? pageIds = null,
            string? orderBy = null);

        Task<IEnumerable<GetNewsCategoryMapping_Result>> GetNewsCategoryMappingAsync(int newsId);
        Task<IEnumerable<GetNewsPageMapping_Result>> GetNewsPageMappingAsync(int newsId);

        Task ReplaceCategoryMappingsAsync(int newsId, IEnumerable<int> categoryIds);
        Task ReplacePageMappingsAsync(int newsId, IEnumerable<int> pageIds);
        Task UpdateNewsUrlAsync(int newsId);
        Task<GetNewsCommonData_Result?> GetNewsCommonDataAsync();
        Task UpdateNewsCommonDataAsync(NewsCommonData entity);
        Task<GetNewsDetailsByUrl_Result?> GetNewsDetailsByUrlAsync(string newsUrl);
    }
}
