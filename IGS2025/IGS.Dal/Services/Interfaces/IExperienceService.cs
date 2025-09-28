using IGS.Models;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Interfaces
{
    public interface IExperienceService
    {
        /// <summary>
        /// Creates/updates Experience and replaces mappings. Returns Experience Id.
        /// </summary>
        Task<int> SaveExperienceAsync(ExperienceModel model);
        Task<bool> DeleteExperienceAsync(int id);
        Task<bool> DeleteLogoAsync(int id, Action<Experience> clearLogoAction);
    }
}
