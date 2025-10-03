using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository.IRepository
{
    public interface ICommonRepository
    {
        Task<GetPageHeader_Result?> GetPageHeaderAsync(string pageName);
        Task<GetOtherContact_Result?> GetOtherContactAsync();
    }
}
