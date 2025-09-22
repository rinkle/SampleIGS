using System.Linq.Expressions;

namespace IGS.Dal.Repository.IRepository
{
    //public interface IRepository<T> where T : class
    //{
    //    // Sync
    //    IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    //    T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
    //    void Add(T entity);
    //    void Remove(T entity);
    //    void RemoveRange(IEnumerable<T> entity);

    //    // Async
    //    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    //    Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
    //    Task AddAsync(T entity);
    //}

    public interface IRepository<T> where T : class
    {
        // Existing
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
        Task AddAsync(T entity);

        // 🚀 New Helper
        Task<T?> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null,
            bool tracked = false);
    }

}
