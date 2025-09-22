using Globalsetting;
using IGS.Models.KeyLessModels;

namespace IGS.Models.ViewModels
{
    public class PortfolioServicessViewModel
    {
        public GetPortfolioService_Result PortfolioServices { get; set; } = new(); // ✅ always initialized
        public List<GetCommonListing_Result> CoreAreasofFocus { get; set; } = new();

        public PortfolioServicessViewModel() { }

        public PortfolioServicessViewModel(GetPortfolioService_Result? PortfolioServicesResult, IEnumerable<GetCommonListing_Result>? allListings = null, bool isAdmin = false)
        {
            // Ensure Home is never null
            PortfolioServices = PortfolioServicesResult ?? new GetPortfolioService_Result();
            CoreAreasofFocus = allListings?
                .Where(x => !string.IsNullOrEmpty(x.Section) &&
                            x.Section.Equals(PageSection.PortfolioServicesCoreAreasofFocus, StringComparison.OrdinalIgnoreCase))
                .OrderBy(o => o.DisplayOrder)
                .ToList()
                ?? new List<GetCommonListing_Result>();

            if (isAdmin)
            {
                CoreAreasofFocus.Add(new GetCommonListing_Result { Id = 0, Section = PageSection.PortfolioServicesCoreAreasofFocus, Fk_PageId = (int)PageEnum.PortfolioServices });
            }
        }
    }
}
