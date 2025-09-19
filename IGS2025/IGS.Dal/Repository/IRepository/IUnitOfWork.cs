namespace IGS.Dal.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IHomeRepository Home { get; }
        ICommonListingRepository CommonListing { get; }
        IPageHeaderRepository PageHeader { get; }
        void Save();
        Task SaveAsync();
    }
}
