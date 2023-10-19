using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ValidationDemoApi.CORE.Interfaces;

namespace ValidationDemoApi.DAL
{
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> GetAllIncluding<TEntity>(
            this IQueryable<TEntity> queryable,
            params Expression<Func<TEntity, object>>[]? includeProperties)
            where TEntity : class
        {
            return includeProperties == null ? queryable : includeProperties.Aggregate(queryable, (current, includeProperty) => current.Include(includeProperty));
        }
    }
    public class EFRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly ContactContext _context;
        private readonly ILogger<EFRepository<T>> _logger;

        public EFRepository(ContactContext context, ILogger<EFRepository<T>> logger)
        {
            _context = context;
            _logger = logger;
        }
        public T Add(T entity)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _context.Add(entity);
                _context.SaveChanges();
                transaction.Commit();
                return entity;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error adding entity");
                transaction.Rollback();                
                throw;
            }
            
        }

        public void Delete(T entity)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Remove(entity);
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting entity");
                transaction.Rollback();
                throw;
            }
            

        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking().ToList();
        }
        
        public IQueryable<TEntity> GetAllIncluding<TEntity>(
            params Expression<Func<TEntity, object>>[]? includeProperties)
            where TEntity : class
        {
            IQueryable<TEntity> queryable = _context.Set<TEntity>();
            return queryable.GetAllIncluding(includeProperties);
        }

        public IEnumerable<T> Filter(Func<T, bool> predicate)
        {
            return _context.Set<T>()
                .AsNoTracking()
                .AsEnumerable()
                .Where(predicate)
                .ToList();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().AsNoTracking().SingleOrDefault(x => x.Id == id) ?? throw new Exception($"Entity of type {typeof(T)} with id {id} not found");
        }

        // add a method to get one entity that matches a predicate
        public T? GetOne(Func<T, bool> predicate)
        {
            return _context.Set<T>().AsNoTracking().FirstOrDefault(predicate);
        }

        public void Update(T entity)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Set<T>().Update(entity);
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating entity");
                transaction.Rollback();
                throw;
            }
            
            
        }
    }
}
