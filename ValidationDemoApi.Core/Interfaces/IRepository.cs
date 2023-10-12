using ValidationDemoApi.CORE.Models;
namespace ValidationDemoApi.CORE.Interfaces
{
    public interface IRepository<T> where T : class, IEntity
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        IEnumerable<T> GetAll(Func<T, bool> predicate);
        T? GetOne(Func<T, bool> predicate);
    }
}
