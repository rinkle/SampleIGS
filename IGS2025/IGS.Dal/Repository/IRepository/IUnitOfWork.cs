namespace IGS.Dal.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IHomeRepository Home { get; }
        void Save();
        Task SaveAsync();
    }
}
