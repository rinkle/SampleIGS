using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models;
using IGS.Models.KeyLessModels;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Implementations
{
    public class NewsVmService : INewsVmService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;

        public NewsVmService(IUnitOfWork unitOfWork, ILoggerService logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Builds NewsViewModel for listing/filtering news.
        /// </summary>
        public async Task<NewsViewModel> GetNewsVmAsync(
            string? categoryIds = null,
            string? pageIds = null,
            string? orderBy = null,
            bool isAdmin = false)
        {
            try
            {
                var newsList = (await _unitOfWork.News
                    .GetNewsFilterListAsync(categoryIds, pageIds, orderBy)).ToList();

                var categories = (await _unitOfWork.News
                    .GetNewsCategoryMappingAsync(0)) // pass 0 to get all categories
                    .ToList();
                var commonDataSp = await _unitOfWork.News.GetNewsCommonDataAsync();

                NewsCommonData commonDataEntity = new();
                if (commonDataSp != null)
                {
                    commonDataEntity = new NewsCommonData
                    {
                        Id = commonDataSp.Id,
                        InsightHeading = commonDataSp.InsightHeading,
                        InsightSubHeading = commonDataSp.InsightSubHeading,
                        FeaturedInsightHeading = commonDataSp.FeaturedInsightHeading,
                        FeaturedInsightSubHeading = commonDataSp.FeaturedInsightSubHeading,
                        FeaturedInsightDescription = commonDataSp.FeaturedInsightDescription,
                        FeaturedInsightImage = commonDataSp.FeaturedInsightImage,
                        FeaturedInsightPdf = commonDataSp.FeaturedInsightPdf,
                        RecentProjectsDescription = commonDataSp.RecentProjectsDescription,
                        RecentProjectsHeading = commonDataSp.RecentProjectsHeading
                    };
                }
                return new NewsViewModel(newsList, categories, commonDataEntity, isAdmin);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in NewsVmService.GetNewsVmAsync");
                return new NewsViewModel();
            }
        }

        /// <summary>
        /// Builds NewsModel for add/edit single news item.
        /// </summary>
        public async Task<NewsModel> GetNewsModelAsync(int newsId, string? categoryIds = null, string? pageIds = null, string? orderBy = null)
        {
            try
            {
                var detail = await _unitOfWork.News
                    .GetNewsDetailByIdAsync(newsId, categoryIds, pageIds, orderBy);

                var categoryMappings = await _unitOfWork.News
                    .GetNewsCategoryMappingAsync(newsId);

                var pageMappings = await _unitOfWork.News
                    .GetNewsPageMappingAsync(newsId);

                return new NewsModel
                {
                    NewsInfo = detail ?? new GetNewsDetail_Result(),
                    NewsCategoryMapping = categoryMappings?.ToList() ?? new(),
                    NewsPageMapping = pageMappings?.ToList() ?? new()
                };
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in NewsVmService.GetNewsModelAsync");
                return new NewsModel();
            }
        }


        /// <summary>
        /// ✅ Builds NewsByUrlModel for single news item fetched by URL (public detail view).
        /// </summary>
        public async Task<NewsByUrlModel> GetNewsModelByUrlAsync(string newsUrl)
        {
            try
            {
                var newsDetail = await _unitOfWork.News.GetNewsDetailsByUrlAsync(newsUrl);

                return new NewsByUrlModel(newsDetail);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in NewsVmService.GetNewsModelByUrlAsync");
                return new NewsByUrlModel();
            }
        }
    }
}
