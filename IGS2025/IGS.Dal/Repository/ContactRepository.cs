using IGS.Dal.Data;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Repository.Repository;
using IGS.Dal.Sql;
using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository
{
    /// <summary>
    /// Repository implementation for Contact entity.
    /// </summary>
    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ISqlHelper _sql;

        public ContactRepository(ApplicationDbContext db, ISqlHelper sql) : base(db)
        {
            _db = db;
            _sql = sql;
        }

        /// <summary>
        /// Update a Contact record.
        /// </summary>
        public void Update(Contact obj)
        {
            _db.Contacts.Update(obj);
        }

        /// <summary>
        /// Executes stored procedure [dbo].[GetContact] and returns active contacts.
        /// </summary>
        public async Task<IEnumerable<GetContact_Result>> GetContactFromSpAsync()
        {
            return await _sql.QueryAsync<GetContact_Result>(
                "dbo.GetContact",
                null,
                isStoredProc: true
            );
        }
    }
}
