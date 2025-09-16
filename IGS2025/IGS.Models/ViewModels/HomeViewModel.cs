using Globalsetting;
using IGS.Models.KeyLessModels;

namespace IGS.Models.ViewModels
{
    public class HomeViewModel
    {
        public GetHome_Result? Home { get; set; }
        public List<GetCommonListing_Result> Carousel { get; set; } = new();

        public HomeViewModel() { }

        public HomeViewModel(GetHome_Result? homeResult, IEnumerable<GetCommonListing_Result>? allListings = null)
        {
            Home = homeResult;

            Carousel = allListings?
                .Where(x => !string.IsNullOrEmpty(x.Section) &&
                            x.Section.Equals(PageSection.HomeCarousel, StringComparison.OrdinalIgnoreCase))
                .OrderBy(o => o.DisplayOrder)
                .ToList()
                ?? new List<GetCommonListing_Result>();
        }
    }
}
