using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Interfaces
{
    public interface IHomeVmService
    {
        Task<HomeViewModel> GetHomeVmAsync(bool isAdmin = false);
    }
}
