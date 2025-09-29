using IGS.Models;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Interfaces
{
    public interface ITeamService
    {
        Task<int> SaveTeamAsync(TeamModel model);
        Task<bool> DeleteTeamAsync(int id);
        Task<bool> DeletePhotoAsync(int id, Action<Team> clearPhotoAction);
    }
}
