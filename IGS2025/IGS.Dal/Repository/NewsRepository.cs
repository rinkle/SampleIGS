using IGS.Dal.Data;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Repository.Repository;
using IGS.Dal.Sql;
using IGS.Models;
using IGS.Models.KeyLessModels;
using Microsoft.EntityFrameworkCore;

namespace IGS.Dal.Repository
{
    public class NewsRepository : Repository<News>, INewsRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ISqlHelper _sql;

        public NewsRepository(ApplicationDbContext db, ISqlHelper sql) : base(db)
        {
            _db = db;
            _sql = sql;
        }

        /// <summary>
        /// Get News list with filters (category, page, orderBy).
        /// </summary>
        public async Task<IEnumerable<GetNewsFilterList_Result>> GetNewsFilterListAsync(
            string? categoryIds = null,
            string? pageIds = null,
            string? orderBy = null)
        {
            return await _sql.QueryAsync<GetNewsFilterList_Result>(
                "dbo.GetNewsFilterList",
                new
                {
                    prmCategoryIds = categoryIds,
                    prmPageIds = pageIds,
                    prmOrderBy = orderBy
                },
                isStoredProc: true);
        }

        /// <summary>
        /// Get detailed News by Id (single record).
        /// </summary>
        public async Task<GetNewsDetail_Result?> GetNewsDetailByIdAsync(
            int newsId,
            string? categoryIds = null,
            string? pageIds = null,
            string? orderBy = null)
        {
            var result = await _sql.QueryAsync<GetNewsDetail_Result>(
                "dbo.GetNewsDetail",
                new
                {
                    prmNewsId = newsId,
                    prmCategoryIds = categoryIds,
                    prmPageIds = pageIds,
                    prmOrderBy = orderBy
                },
                isStoredProc: true);

            return result.FirstOrDefault();
        }

        /// <summary>
        /// Get category mappings for NewsId.
        /// </summary>
        public async Task<IEnumerable<GetNewsCategoryMapping_Result>> GetNewsCategoryMappingAsync(int newsId)
        {
            return await _sql.QueryAsync<GetNewsCategoryMapping_Result>(
                "dbo.GetNewsCategoryMapping",
                new { NewsId = newsId },
                isStoredProc: true);
        }

        /// <summary>
        /// Get page mappings for NewsId.
        /// </summary>
        public async Task<IEnumerable<GetNewsPageMapping_Result>> GetNewsPageMappingAsync(int newsId)
        {
            return await _sql.QueryAsync<GetNewsPageMapping_Result>(
                "dbo.GetNewsPageMapping",
                new { NewsId = newsId },
                isStoredProc: true);
        }

        /// <summary>
        /// Replace category mappings for a NewsId.
        /// </summary>
        public async Task ReplaceCategoryMappingsAsync(int newsId, IEnumerable<int> categoryIds)
        {
            var old = _db.Set<NewsCategoryMapping>().Where(x => x.FK_NewsId == newsId);
            _db.Set<NewsCategoryMapping>().RemoveRange(old);

            foreach (var id in categoryIds)
            {
                await _db.Set<NewsCategoryMapping>().AddAsync(new NewsCategoryMapping
                {
                    FK_NewsId = newsId,
                    FK_NewsCategoryId = id
                });
            }

            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Replace page mappings for a NewsId.
        /// </summary>
        public async Task ReplacePageMappingsAsync(int newsId, IEnumerable<int> pageIds)
        {
            var old = _db.Set<NewsPageMapping>().Where(x => x.FK_NewsId == newsId);
            _db.Set<NewsPageMapping>().RemoveRange(old);

            foreach (var id in pageIds)
            {
                await _db.Set<NewsPageMapping>().AddAsync(new NewsPageMapping
                {
                    FK_NewsId = newsId,
                    FK_PageId = id
                });
            }

            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Runs SP to update NewsUrl.
        /// </summary>
        public async Task UpdateNewsUrlAsync(int newsId)
        {
            await _sql.ExecuteAsync(
                "dbo.UpdateNewsUrl", // assuming you’ll create this SP same as UpdateExperienceUrl
                new { NewsId = newsId },
                isStoredProc: true
            );
        }

        public async Task<GetNewsCommonData_Result?> GetNewsCommonDataAsync()
        {
            var rows = await _sql.QueryAsync<GetNewsCommonData_Result>(
                "dbo.GetNewsCommonData",
                null,
                isStoredProc: true);

            return rows.FirstOrDefault();
        }
        public async Task UpdateNewsCommonDataAsync(NewsCommonData entity)
        {
            var existing = await _db.NewsCommonData.FirstOrDefaultAsync();
            if (existing != null)
            {
                existing.InsightHeading = entity.InsightHeading;
                existing.InsightSubHeading = entity.InsightSubHeading;
                existing.FeaturedInsightHeading = entity.FeaturedInsightHeading;
                existing.FeaturedInsightSubHeading = entity.FeaturedInsightSubHeading;
                existing.FeaturedInsightDescription = entity.FeaturedInsightDescription;
                existing.FeaturedInsightImage = entity.FeaturedInsightImage;

                _db.NewsCommonData.Update(existing);
            }
            else
            {
                await _db.NewsCommonData.AddAsync(entity);
            }
            await _db.SaveChangesAsync();
        }


        /// <summary>
        /// ✅ Get detailed News by URL (single record, for public view).
        /// </summary>
        public async Task<GetNewsDetailsByUrl_Result?> GetNewsDetailsByUrlAsync(string newsUrl)
        {
            var result = await _sql.QueryAsync<GetNewsDetailsByUrl_Result>(
                "dbo.GetNewsDetailsByUrl",
                new { prmNewsUrl = newsUrl },
                isStoredProc: true);

            return result.FirstOrDefault();
        }
    }
}
