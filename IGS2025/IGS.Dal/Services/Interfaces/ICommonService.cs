using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Interfaces
{
    public interface ICommonService
    {
        Task<CommonHeaderFooterModel> GetCommonServiceAsync(string pageName);
    }
}
