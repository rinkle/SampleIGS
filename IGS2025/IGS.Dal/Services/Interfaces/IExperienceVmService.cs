using IGS.Models;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Interfaces
{
    public interface IExperienceVmService
    {
        Task<ExperienceViewModel> GetExperienceVmAsync(
            string? industryCategoryIds = null,
            string? pageIds = null,
            string? orderBy = null,
            bool isAdmin = false);
        Task<ExperienceModel> GetExperienceModelAsync(
             int experienceId,
             string? industryCategoryIds = null,
             string? pageIds = null,
             string? orderBy = null);
    }
}
