using IGS.Models.ViewModels;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Services.Interfaces
{
    public interface ITeamTitleVmService
    {
        Task<TeamTitleViewModel> GetTeamTitleVmAsync(bool isAdmin = false);
        Task<GetTeamTitle_Result> GetTeamTitleDetailAsync(int id);
    }
}
