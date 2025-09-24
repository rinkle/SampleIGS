using IGS.Dal.Data;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Repository.Repository;
using IGS.Dal.Sql;
using IGS.Models;
using IGS.Models.KeyLessModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IGS.Dal.Repository
{
    public class IndustryRepository : Repository<Industry>, IIndustryRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ISqlHelper _sql;

        public IndustryRepository(ApplicationDbContext db, ISqlHelper sql) : base(db)
        {
            _db = db;
            _sql = sql;
        }

        /// <summary>
        /// Update an existing Industry entity.
        /// </summary>
        /// <param name="obj">The Industry entity to update</param>
        public void Update(Industry obj)
        {
            _db.Industries.Update(obj);
        }

        /// <summary>
        /// Executes stored procedure [dbo].[GetIndustry] and returns the first result.
        /// </summary>
        public async Task<GetIndustry_Result?> GetIndustryFromSpAsync()
        {
            try
            {
                var result = await _sql.QueryAsync<GetIndustry_Result>(
                    "dbo.GetIndustry",
                    isStoredProc: true
                );

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Optionally log exception here
                throw;
            }
        }


        public async Task<IEnumerable<GetIndustryCategory_Result>> GetIndustryCategoryFromSpAsync()
        {
            try
            {
                return await _sql.QueryAsync<GetIndustryCategory_Result>(
                  "dbo.GetIndustryCategory",
                  isStoredProc: true
           );
            }
            catch (Exception Ex)
            {
                throw;
            }

        }
    }
}
