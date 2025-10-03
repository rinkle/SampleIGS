using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Interfaces
{
    public interface IContactVmService
    {
        /// <summary>
        /// Returns a view model containing the list of active contacts.
        /// </summary>
        Task<ContactViewModel> GetContactVmAsync(bool isAdmin = false);
    }
}
