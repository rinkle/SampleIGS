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

        public NewsViewModel() { }

        public NewsViewModel(IEnumerable<GetNewsFilterList_Result>? newsList,
                             IEnumerable<GetNewsCategoryMapping_Result>? categories,
                             bool isAdmin = false)
        {
            NewsList = newsList?
                .OrderBy(x => x.DisplayOrder)
                .ThenByDescending(x => x.NewsDate)
                .ToList()
                ?? new List<GetNewsFilterList_Result>();

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
