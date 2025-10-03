using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Interfaces
{
    public interface IContactService
    {
        Task SaveContactAsync(ContactViewModel model);
    }
}
