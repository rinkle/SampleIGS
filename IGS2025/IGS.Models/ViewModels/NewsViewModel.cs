using IGS.Models.KeyLessModels;

namespace IGS.Models.ViewModels
{
    /// <summary>
    /// ViewModel for News list/filter UI.
    /// </summary>
    public class NewsViewModel
    {
        public List<GetNewsFilterList_Result> NewsList { get; set; } = new();
        public List<GetNewsCategoryMapping_Result> NewsCategories { get; set; } = new();
        public NewsCommonData NewsCommonData { get; set; } = new();

        public NewsViewModel() { }

        public NewsViewModel(IEnumerable<GetNewsFilterList_Result>? newsList,
                             IEnumerable<GetNewsCategoryMapping_Result>? categories,
                             NewsCommonData? commonData = null,
                             bool isAdmin = false)
        { 
            NewsList = newsList?
                .OrderByDescending(x => x.DisplayOrder)
                .ThenByDescending(x => x.NewsDate)
                .ToList()
                ?? new List<GetNewsFilterList_Result>();
            NewsCommonData = commonData ?? new NewsCommonData();

            if (!isAdmin)
            {
                NewsCategories = categories?
                    .OrderBy(x => x.DisplayOrder)
                    .ToList()
                    ?? new List<GetNewsCategoryMapping_Result>();
            }
        }
    }
}
