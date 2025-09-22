using Globalsetting;
using IGS.Models.KeyLessModels;

namespace IGS.Models.ViewModels
{
    public class TransactionServicesViewModel
    {
        public GetTransactionService_Result TransactionService { get; set; } = new(); // ✅ always initialized
        public List<GetCommonListing_Result> CoreAreasofFocus { get; set; } = new();

        public TransactionServicesViewModel() { }

        public TransactionServicesViewModel(GetTransactionService_Result? transactionServiceResult, IEnumerable<GetCommonListing_Result>? allListings = null, bool isAdmin = false)
        {
            // Ensure Home is never null
            TransactionService = transactionServiceResult ?? new GetTransactionService_Result();

            // Ensure Carousel is never null
            CoreAreasofFocus = allListings?
                .Where(x => !string.IsNullOrEmpty(x.Section) &&
                            x.Section.Equals(PageSection.TransactionServicesCoreAreasofFocus, StringComparison.OrdinalIgnoreCase))
                .OrderBy(o => o.DisplayOrder)
                .ToList()
                ?? new List<GetCommonListing_Result>();

            if (isAdmin)
            {
                CoreAreasofFocus.Add(new GetCommonListing_Result { Id = 0, Section = PageSection.HomeAtAGlance, Fk_PageId = (int)PageEnum.Home });
            }
        }
    }
}
