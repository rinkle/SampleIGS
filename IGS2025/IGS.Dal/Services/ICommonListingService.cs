using IGS.Models.KeyLessModels;

namespace IGS.Dal.Services
{
    public interface ICommonListingService
    {
        Task SaveCommonListingAsync(List<GetCommonListing_Result> commonListItems);
    }
}
