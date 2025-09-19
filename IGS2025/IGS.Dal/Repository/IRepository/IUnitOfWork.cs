namespace IGS.Dal.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IHomeRepository Home { get; }
        ICommonListingRepository CommonListing { get; }
        IPageHeaderRepository PageHeader { get; }
        IPageRepository Page { get; }

        void Save();
        Task SaveAsync();
    }
}
