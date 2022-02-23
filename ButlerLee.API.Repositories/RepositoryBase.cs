using ButlerLee.API.Contracts.IRepositories;
using ButlerLee.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ButlerLee.API.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext { get; set; }

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        public async Task<IQueryable<T>> FindAllAsync()
        {
            List<T> entities = await RepositoryContext.Set<T>().ToListAsync();
            return entities.AsQueryable();
        }

        public async Task<IQueryable<T>> FindByAsync(Expression<Func<T, bool>> expression)
        {
            List<T> entities = await RepositoryContext.Set<T>().Where(expression).ToListAsync();
            return entities.AsQueryable();
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await RepositoryContext.Set<T>().SingleOrDefaultAsync(expression);
        }

        public void Create(T entity)
        {
            RepositoryContext.Set<T>().Add(entity);
        }

        public void CreateRange(IEnumerable<T> entity)
        {
            RepositoryContext.Set<T>().AddRange(entity);
        }

        public void Update(T entity, object key)
        {
            T exist = RepositoryContext.Set<T>().Find(key);
            if (exist != null)
            {
                RepositoryContext.Entry(exist).CurrentValues.SetValues(entity);
            }
        }

        public void Delete(T entity)
        {
            RepositoryContext.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            RepositoryContext.Set<T>().RemoveRange(entities);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    RepositoryContext.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
