using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Interfaces
{
    /// <summary>
    /// ViewModel service interface for loading newsletter data across all pages.
    /// </summary>
    public interface INewsLetterVmService
    {
        /// <summary>
        /// Returns the Newsletter view model (includes heading and script from GetHome).
        /// </summary>
        Task<NewsLetterViewModel> GetNewsLetterViewModelAsync();
    }
}
