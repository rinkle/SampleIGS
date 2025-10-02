using IGS.Models.KeyLessModels;

namespace IGS.Models
{
    /// <summary>
    /// Model for add/edit News (single record + mappings).
    /// </summary>
    public class NewsModel
    {
        public GetNewsDetail_Result NewsInfo { get; set; } = new();
        public List<GetNewsCategoryMapping_Result> NewsCategoryMapping { get; set; } = new();
        public List<GetNewsPageMapping_Result> NewsPageMapping { get; set; } = new();

        public NewsModel() { }

        public NewsModel(GetNewsDetail_Result? newsInfo,
                         IEnumerable<GetNewsCategoryMapping_Result>? categoryMappings,
                         IEnumerable<GetNewsPageMapping_Result>? pageMappings)
        {
            NewsInfo = newsInfo ?? new GetNewsDetail_Result();
            NewsCategoryMapping = categoryMappings?.ToList() ?? new List<GetNewsCategoryMapping_Result>();
            NewsPageMapping = pageMappings?.ToList() ?? new List<GetNewsPageMapping_Result>();
        }
    }
}
