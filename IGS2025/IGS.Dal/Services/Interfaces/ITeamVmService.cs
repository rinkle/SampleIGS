using IGS.Models;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Interfaces
{
    public interface ITeamVmService
    {
        Task<TeamViewModel> GetTeamVmAsync(
            string? categoryIds = null,
            string? locationIds = null,
            string? orderBy = null,
            bool isAdmin = false);

        Task<TeamModel> GetTeamModelAsync(int teamId);
    }
}
