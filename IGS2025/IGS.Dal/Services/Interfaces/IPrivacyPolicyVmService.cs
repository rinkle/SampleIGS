using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Interfaces
{
    public interface IPrivacyPolicyVmService
    {
        /// <summary>
        /// Returns a view model containing the list of privacy policies filtered by PageName.
        /// </summary>
        Task<PrivacyPolicyViewModel> GetPrivacyPolicyVmAsync(string pageName, bool isAdmin = false);
    }
}
