using IGS.Dal.Data;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Repository.Repository;
using IGS.Dal.Sql;
using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository
{
    public class PrivacyPolicyRepository : Repository<PrivacyPolicy>, IPrivacyPolicyRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ISqlHelper _sql;

        public PrivacyPolicyRepository(ApplicationDbContext db, ISqlHelper sql) : base(db)
        {
            _db = db;
            _sql = sql;
        }

        public void Update(PrivacyPolicy obj)
        {
            _db.PrivacyPolicies.Update(obj);
        }

        public async Task<IEnumerable<GetPrivacyPolicy_Result>> GetPrivacyPolicyFromSpAsync(string pageName)
        {
            return await _sql.QueryAsync<GetPrivacyPolicy_Result>(
                "dbo.GetPrivacyPolicy",
                new { PageName = pageName },
                isStoredProc: true
            );
        }
    }
}
