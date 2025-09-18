using Globalsetting;
using IGS.Models.KeyLessModels;

namespace IGS.Models.ViewModels
{
    public class HomeViewModel
    {
        public GetHome_Result Home { get; set; } = new(); // ✅ always initialized
        public List<GetCommonListing_Result> Carousel { get; set; } = new();
        public List<GetCommonListing_Result> AtAGlance { get; set; } = new();
        public List<GetCommonListing_Result> TransactionsandGrowth { get; set; } = new();
        public List<GetCommonListing_Result> CoreAreasoFocus { get; set; } = new();




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

            TransactionsandGrowth = allListings?
               .Where(x => !string.IsNullOrEmpty(x.Section) &&
                           x.Section.Equals(PageSection.HomeTransactionsandGrowth, StringComparison.OrdinalIgnoreCase))
               .OrderBy(o => o.DisplayOrder)
               .ToList()
               ?? new List<GetCommonListing_Result>();

            CoreAreasoFocus = allListings?
              .Where(x => !string.IsNullOrEmpty(x.Section) &&
                          x.Section.Equals(PageSection.HomeCoreAreasofFocus, StringComparison.OrdinalIgnoreCase))
              .OrderBy(o => o.DisplayOrder)
              .ToList()
              ?? new List<GetCommonListing_Result>();

            if (isAdmin)
            {
                // Always add one empty row for admin users
                Carousel.Add(new GetCommonListing_Result { Id = 0, Section = PageSection.HomeCarousel, Fk_PageId = (int)PageEnum.Home });
                if (AtAGlance.Count<3)
                {
                    AtAGlance.Add(new GetCommonListing_Result { Id = 0, Section = PageSection.HomeAtAGlance, Fk_PageId = (int)PageEnum.Home });
                }
                if (TransactionsandGrowth.Count<3)
                {
                    TransactionsandGrowth.Add(new GetCommonListing_Result { Id = 0, Section = PageSection.HomeTransactionsandGrowth, Fk_PageId = (int)PageEnum.Home });
                }
                if (CoreAreasoFocus.Count<6)
                {
                    CoreAreasoFocus.Add(new GetCommonListing_Result { Id = 0, Section = PageSection.HomeCoreAreasofFocus, Fk_PageId = (int)PageEnum.Home });
                }
                //


            }
        }
    }
}
