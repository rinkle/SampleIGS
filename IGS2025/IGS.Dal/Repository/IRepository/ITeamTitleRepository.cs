using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository.IRepository
{
    public interface ITeamTitleRepository : IRepository<TeamTitle>
    {
        // SP-based reads
        Task<IEnumerable<GetTeamTitle_Result>> GetTeamTitleListAsync();
        Task<GetTeamTitle_Result?> GetTeamTitleDetailByIdAsync(int id);

        // Extra helper (like Experience)
        Task UpdateTeamTitleUrlAsync(int id);
    }
}
