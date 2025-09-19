using IGS.Models.KeyLessModels;

namespace IGS.Models.ViewModels
{
    public class PageHeaderModel
    {
        public GetPageHeader_Result PageHeader { get; set; } = new();
        public List<GetCommonListing_Result> HeaderCarousel { get; set; } = new();
    }
}
