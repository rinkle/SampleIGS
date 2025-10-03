using IGS.Dal.Data;   // ✅ Correct namespace
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Sql;
using IGS.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace IGS.Dal.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private readonly ISqlHelper _sql;

        public IHomeRepository Home { get; private set; }
        public ICommonListingRepository CommonListing { get; private set; }
        public IPageHeaderRepository PageHeader { get; private set; }
        public IPageRepository Page { get; private set; }
        public ITransactionServiceRepository TransactionService { get; private set; }
        public IPortfolioServiceRepository PortfolioService { get; private set; }
        public IIndustryRepository IndustryService { get; private set; }
        public IIndustryCategoryRepository IndustryCategory { get; private set; }
        public IExperienceRepository Experience { get; private set; }
        public ITeamRepository Team { get; private set; }
        public ITeamCategoryRepository TeamCategory { get; private set; }
        public ITeamTitleRepository TeamTitle { get; private set; }
        public INewsRepository News { get; private set; }
        public INewsCommonDataRepository NewsCommonData { get; private set; }
        public IContactRepository Contact { get; private set; }

        public UnitOfWork(ApplicationDbContext db, ISqlHelper sql)
        {
            _db = db;
            _sql = sql;

            Home = new HomeRepository(_db, _sql);
            CommonListing = new CommonListingRepository(_db, _sql);
            PageHeader = new PageHeaderRepository(_db, _sql);
            Page = new PageRepository(_db, _sql);
            TransactionService = new TransactionServiceRepository(_db, _sql);
            PortfolioService = new PortfolioServiceRepository(_db, _sql);
            IndustryService = new IndustryRepository(_db, _sql);
            IndustryCategory = new IndustryCategoryRepository(_db, _sql);
            Experience = new ExperienceRepository(_db, _sql);
            Team = new TeamRepository(_db, _sql);
            TeamCategory = new TeamCategoryRepository(_db, _sql);
            TeamTitle = new TeamTitleRepository(_db, _sql);
            News = new NewsRepository(_db, _sql);
            NewsCommonData = new NewsCommonDataRepository(_db);
            Contact = new ContactRepository(_db, _sql);
        }

        public void Save() => _db.SaveChanges();
        public async Task SaveAsync() => await _db.SaveChangesAsync();
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _db.Database.BeginTransactionAsync();
        }
    }
}
