using Globalsetting;
using IGS.Models.KeyLessModels;

namespace IGS.Models.ViewModels
{
    public class HomeViewModel
    {
        public GetHome_Result Home { get; set; } = new(); // ✅ always initialized
        public List<GetCommonListing_Result> Carousel { get; set; } = new();
        public List<GetCommonListing_Result> AtAGlance { get; set; } = new();


        public HomeViewModel() { }

        public HomeViewModel(GetHome_Result? homeResult, IEnumerable<GetCommonListing_Result>? allListings = null, bool isAdmin = false)
        {
            // Ensure Home is never null
            Home = homeResult ?? new GetHome_Result();

            // Ensure Carousel is never null
            Carousel = allListings?
                .Where(x => !string.IsNullOrEmpty(x.Section) &&
                            x.Section.Equals(PageSection.HomeCarousel, StringComparison.OrdinalIgnoreCase))
                .OrderBy(o => o.DisplayOrder)
                .ToList()
                ?? new List<GetCommonListing_Result>();

            AtAGlance = allListings?
                .Where(x => !string.IsNullOrEmpty(x.Section) &&
                            x.Section.Equals(PageSection.HomeAtAGlance, StringComparison.OrdinalIgnoreCase))
                .OrderBy(o => o.DisplayOrder)
                .ToList()
                ?? new List<GetCommonListing_Result>();

            if (isAdmin)
            {
                // Always add one empty row for admin users
                Carousel.Add(new GetCommonListing_Result { Id = 0, Section = PageSection.HomeCarousel, Fk_PageId = (int)PageEnum.Home });
            }
        }
    }
}
