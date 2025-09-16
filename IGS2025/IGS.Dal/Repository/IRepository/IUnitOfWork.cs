namespace IGS.Dal.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IHomeRepository Home { get; }
        ICommonListingRepository CommonListing { get; }
        void Save();
        Task SaveAsync();
    }
}
