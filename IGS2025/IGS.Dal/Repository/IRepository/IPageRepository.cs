using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Repository.IRepository
{
    public interface IPageRepository : IRepository<Page>
    {
        void Update(Page obj);
        
    }
}
