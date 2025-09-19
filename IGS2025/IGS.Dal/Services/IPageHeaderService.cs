using IGS.Dal.Sql;
using IGS.Models.ViewModels;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Services
{
    public interface IPageHeaderService
    {
        Task<PageHeaderModel> GetPageHeaderAsync(string pageName, int pageId, CancellationToken ct = default);
    }
}
