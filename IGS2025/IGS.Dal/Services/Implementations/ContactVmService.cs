using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Implementations
{
    public class ContactVmService : IContactVmService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;

        public ContactVmService(IUnitOfWork unitOfWork, ILoggerService logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ContactViewModel> GetContactVmAsync(bool isAdmin = false)
        {
            try
            {
                var contacts = await _unitOfWork.Contact.GetContactFromSpAsync();
                return new ContactViewModel(contacts);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error fetching Contact data");
                return new ContactViewModel();
            }
        }
    }
}
