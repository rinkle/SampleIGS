using IGS.Models.ViewModels;
using IGS.ViewModels;

namespace IGS.Dal.Services.Interfaces
{
    public interface IIndustryVmService
    {
        /// <summary>
        /// Builds an IndustryViewModel using stored procedure data.
        /// </summary>
        Task<IndustryViewModel> GetIndustryVmAsync(bool isAdmin = false);
    }
}
