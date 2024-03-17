using SearchAPI.Models;

namespace SearchAPI.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        void Insert(T entity);
        void Save();
    }
}
