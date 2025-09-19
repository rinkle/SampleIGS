using Globalsetting;
using IGS.Models.KeyLessModels;

namespace IGS.Models.ViewModels
{
    public class PageHeaderModel
    {
        public GetPageHeader_Result PageHeader { get; set; } = new();
        public List<GetCommonListing_Result> HeaderCarousel { get; set; } = new();
        public PageHeaderModel()
        {
                
        }
        public PageHeaderModel(GetPageHeader_Result? pageResult, IEnumerable<GetCommonListing_Result>? allListings = null, bool isAdmin = false)
        {
            PageHeader = pageResult??new GetPageHeader_Result();
            HeaderCarousel = allListings?
                .Where(x => !string.IsNullOrEmpty(x.Section) &&
                            x.Section.Equals(PageSection.PageHeader, StringComparison.OrdinalIgnoreCase))
                .OrderBy(o => o.DisplayOrder)
                .ToList()
                ?? new List<GetCommonListing_Result>();
            if (isAdmin)
            {
                HeaderCarousel.Add(new GetCommonListing_Result { Id = 0, Section = PageSection.PageHeader, Fk_PageId = (int)PageEnum.Home });

            }
        }
    }
}
