using Microsoft.EntityFrameworkCore.Storage;

namespace IGS.Dal.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IHomeRepository Home { get; }
        ICommonListingRepository CommonListing { get; }
        IPageHeaderRepository PageHeader { get; }
        IPageRepository Page { get; }
        ITransactionServiceRepository TransactionService { get; }
        IPortfolioServiceRepository PortfolioService { get; }
        IIndustryRepository IndustryService { get; }
        IIndustryCategoryRepository IndustryCategory { get; }
        IExperienceRepository Experience { get; }
        ITeamRepository Team { get; }
        ITeamCategoryRepository TeamCategory { get; }

        void Save();
        Task SaveAsync();

        // ✅ New
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
