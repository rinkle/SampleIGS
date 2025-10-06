using IGS.Models.KeyLessModels;

namespace IGS.Models.ViewModels
{
    public class NewsLetterViewModel
    {
        public GetHome_Result NewsLetterInfo { get; set; } = new();

        public NewsLetterViewModel() { }

        public NewsLetterViewModel(GetHome_Result? homeResult)
        {
            NewsLetterInfo = homeResult ?? new GetHome_Result();
        }
    }
}
