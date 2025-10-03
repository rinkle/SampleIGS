using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Interfaces
{
    public interface IPrivacyPolicyService
    {
        /// <summary>
        /// Saves the privacy policy data for a given PageName.
        /// </summary>
        Task SavePrivacyPolicyAsync(PrivacyPolicyViewModel model, string pageName);
    }
}
