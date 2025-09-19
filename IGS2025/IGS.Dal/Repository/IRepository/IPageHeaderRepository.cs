using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository.IRepository
{
    public interface IPageHeaderRepository : IRepository<PageHeader>
    {
        void Update(PageHeader obj);
        Task<GetPageHeader_Result?> GetPageHeaderFromSpAsync(string pageName);
    }
}
