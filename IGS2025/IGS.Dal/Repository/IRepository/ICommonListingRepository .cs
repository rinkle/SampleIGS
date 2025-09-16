using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository.IRepository
{
    public interface ICommonListingRepository : IRepository<CommonListing>
    {
        void Update(CommonListing entity);

        // Example: call stored procedure to get listings
        Task<IEnumerable<GetCommonListing_Result>> GetCommonListingFromSpAsync(int pageId);
    }
}
