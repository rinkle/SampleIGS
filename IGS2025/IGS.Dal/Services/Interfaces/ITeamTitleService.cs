using IGS.Models.KeyLessModels;

namespace IGS.Dal.Services.Interfaces
{
    public interface ITeamTitleService
    {
        Task<int> SaveTeamTitleAsync(GetTeamTitle_Result model);
        Task<bool> DeleteTeamTitleAsync(int id);
    }
}
