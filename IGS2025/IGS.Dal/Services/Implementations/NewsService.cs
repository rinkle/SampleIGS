using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models;
using IGS.Models.KeyLessModels;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Implementations
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly GlobalEnvironmentSetting _env;

        public NewsService(
            IUnitOfWork unitOfWork,
            ILoggerService logger,
            GlobalEnvironmentSetting env)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// Creates or updates a News record.
        /// </summary>
        public async Task<int> SaveNewsAsync(NewsModel model)
        {
            if (model == null || model.NewsInfo == null) return 0;

            try
            {
                var incoming = model.NewsInfo;
                News entity;

                if (incoming.NewsId > 0)
                {
                    entity = await _unitOfWork.News.GetAsync(
                        n => n.NewsId == incoming.NewsId, tracked: true);

                    if (entity == null)
                    {
                        entity = new News();
                        await _unitOfWork.News.AddAsync(entity);
                    }

                    MapNews(entity, incoming);
                    entity.ModifiedBy = _env.UserId;
                    entity.ModifiedDate = DateTime.Now;

                    _unitOfWork.News.Update(entity);
                }
                else
                {
                    entity = new News();
                    MapNews(entity, incoming);

                    entity.CreatedBy = _env.UserId;
                    entity.CreatedDate = DateTime.Now;
                    entity.ModifiedBy = _env.UserId;
                    entity.ModifiedDate = DateTime.Now;

                    await _unitOfWork.News.AddAsync(entity);
                }

                // Save main News entity
                await _unitOfWork.SaveAsync();

                // Update NewsUrl using SP
                await _unitOfWork.News.UpdateNewsUrlAsync(entity.NewsId);

                // Replace category mappings
                if (model.NewsCategoryMapping?.Any() == true)
                {
                    var selectedCategoryIds = model.NewsCategoryMapping
                        .Where(x => x.CheckedStatus)
                        .Select(x => x.CategoryId)
                        .ToList();

                    await _unitOfWork.News.ReplaceCategoryMappingsAsync(entity.NewsId, selectedCategoryIds);
                }

                // Replace page mappings
                if (model.NewsPageMapping?.Any() == true)
                {
                    var selectedPageIds = model.NewsPageMapping
                        .Where(x => x.CheckedStatus)
                        .Select(x => x.PageId)
                        .ToList();

                    await _unitOfWork.News.ReplacePageMappingsAsync(entity.NewsId, selectedPageIds);
                }

                return entity.NewsId;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in NewsService.SaveNewsAsync");
                throw;
            }
        }

        /// <summary>
        /// Soft delete (IsActive = false).
        /// </summary>
        public async Task<bool> DeleteNewsAsync(int id)
        {
            try
            {
                var news = await _unitOfWork.News.GetAsync(x => x.NewsId == id, tracked: true);

                if (news == null)
                    return false;

                news.IsActive = false;
                news.ModifiedBy = _env.UserId;
                news.ModifiedDate = DateTime.Now;

                _unitOfWork.News.Update(news);
                await _unitOfWork.SaveAsync();

                await _unitOfWork.News.UpdateNewsUrlAsync(news.NewsId);

                return true;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, $"Error deleting News {id}");
                throw;
            }
        }

        /// <summary>
        /// Clears a logo field (e.g., entity.Logo = null).
        /// </summary>
        public async Task<bool> DeleteLogoAsync(int id, Action<News> clearLogoAction)
        {
            try
            {
                var news = await _unitOfWork.News.GetAsync(
                    n => n.NewsId == id, tracked: true);

                if (news == null)
                    return false;

                clearLogoAction(news);

                news.ModifiedBy = _env.UserId;
                news.ModifiedDate = DateTime.Now;

                _unitOfWork.News.Update(news);
                await _unitOfWork.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, $"Error deleting logo for News {id}");
                throw;
            }
        }

        /// <summary>
        /// Maps SP result to entity.
        /// </summary>
        private static void MapNews(News target, GetNewsDetail_Result src)
        {
            target.NewsDate = src.NewsDate == default ? DateTime.Now : src.NewsDate;
            target.NewsHeadLine = src.NewsHeadLine?.Trim();
            target.Logo = src.Logo;
            target.SortDescription = src.SortDescription;
            target.KeyInsight = src.KeyInsight;
            target.BottomText = src.BottomText;
            target.Description = src.Description;
            target.PdfFileName = src.PdfFileName;
            target.NewsType = src.NewsType;
            target.ExternalLink = src.ExternalLink;
            target.NewsUrl = src.NewsUrl;
            target.DisplayOrder = src.DisplayOrder;
            target.IsActive = src.IsActive ?? true;
        }

        public async Task<GetNewsCommonData_Result?> GetNewsCommonDataAsync()
        {
            try
            {
                return await _unitOfWork.News.GetNewsCommonDataAsync();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error fetching NewsCommonData");
                throw;
            }
        }
        // NEW: Creates or updates NewsCommonData
        public async Task SaveNewsCommonDataAsync(NewsCommonData model)
        {
            if (model == null) return;

            try
            {
                var existing = await _unitOfWork.NewsCommonData.GetAsync(
                    n => n.Id == model.Id, tracked: true);

                if (existing != null)
                {
                    // update
                    existing.InsightHeading = model.InsightHeading;
                    existing.InsightSubHeading = model.InsightSubHeading;
                    existing.FeaturedInsightHeading = model.FeaturedInsightHeading;
                    existing.FeaturedInsightSubHeading = model.FeaturedInsightSubHeading;
                    existing.FeaturedInsightDescription = model.FeaturedInsightDescription;
                    existing.FeaturedInsightImage = model.FeaturedInsightImage;
                    existing.FeaturedInsightPdf = model.FeaturedInsightPdf;
                    existing.RecentProjectsHeading = model.RecentProjectsHeading;
                    existing.RecentProjectsDescription = model.RecentProjectsDescription;
                    _unitOfWork.NewsCommonData.Update(existing);
                }
                else
                {
                    await _unitOfWork.NewsCommonData.AddAsync(model);
                }

                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in NewsService.SaveNewsCommonDataAsync");
                throw;
            }
        }
    }
}
