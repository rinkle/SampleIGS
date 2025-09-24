using Globalsetting;
using IGS.Models.KeyLessModels;

namespace IGS.Models.ViewModels
{
    /// <summary>
    /// ViewModel for Portfolio Services page.
    /// </summary>
    public class PortfolioServicesViewModel
    {
        public GetPortfolioService_Result PortfolioServices { get; set; } = new();
        public List<GetCommonListing_Result> CoreAreasOfFocus { get; set; } = new();

        public PortfolioServicesViewModel() { }

        public PortfolioServicesViewModel(GetPortfolioService_Result? portfolioServiceResult,
                                          IEnumerable<GetCommonListing_Result>? allListings = null,
                                          bool isAdmin = false)
        {
            PortfolioServices = portfolioServiceResult ?? new GetPortfolioService_Result();

            CoreAreasOfFocus = allListings?
                .Where(x => !string.IsNullOrEmpty(x.Section) &&
                            x.Section.Equals(PageSection.PortfolioServicesCoreAreasofFocus, StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.DisplayOrder)
                .ToList()
                ?? new List<GetCommonListing_Result>();

            if (isAdmin)
            {
                CoreAreasOfFocus.Add(new GetCommonListing_Result
                {
                    Id = 0,
                    Section = PageSection.PortfolioServicesCoreAreasofFocus,
                    Fk_PageId = (int)PageEnum.PortfolioServices
                });
            }
        }
    }
}
