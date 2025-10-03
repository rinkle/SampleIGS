using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository.IRepository
{
    public interface IPrivacyPolicyRepository : IRepository<PrivacyPolicy>
    {
        void Update(PrivacyPolicy obj);
        Task<IEnumerable<GetPrivacyPolicy_Result>> GetPrivacyPolicyFromSpAsync(string pageName);
    }
}
