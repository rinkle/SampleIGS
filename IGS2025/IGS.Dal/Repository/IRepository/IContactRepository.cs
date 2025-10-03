using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository.IRepository
{
    public interface IContactRepository : IRepository<Contact>
    {
        void Update(Contact obj);
        Task<IEnumerable<GetContact_Result>> GetContactFromSpAsync();
    }
}
