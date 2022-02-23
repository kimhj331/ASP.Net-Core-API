using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ButlerLee.API.Contracts.IRepositories
{
    public interface IRepositoryBase<T>
    {
        Task<IQueryable<T>> FindAllAsync();
        Task<IQueryable<T>> FindByAsync(Expression<Func<T, bool>> expression);
        Task<T> FindAsync(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void CreateRange(IEnumerable<T> entity);
        void Update(T entity, object key);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Dispose();
    }
}
