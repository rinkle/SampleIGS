using IGS.Models.KeyLessModels;

namespace IGS.Models.ViewModels
{
    /// <summary>
    /// ViewModel for a single News detail item fetched by its URL.
    /// Used for public-facing News Detail pages.
    /// </summary>
    public class NewsByUrlModel
    {
        public GetNewsDetailsByUrl_Result NewsInfo { get; set; } = new();

        public NewsByUrlModel() { }

        public NewsByUrlModel(GetNewsDetailsByUrl_Result? newsInfo)
        {
            NewsInfo = newsInfo ?? new GetNewsDetailsByUrl_Result();
        }
    }
}
