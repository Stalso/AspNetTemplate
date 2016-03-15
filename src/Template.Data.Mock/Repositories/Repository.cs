using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Template.Domain.Entities.Interfaces;
using Template.Domain.Repositories;

namespace Template.Data.Mock.Repositories
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        private DbSet<TEntity> _collection { get; set; }
        public Repository(DbSet<TEntity> collection)
        {
            _collection = collection;
        }
        public Task AddAsync(List<TEntity> entities, CancellationToken cancellationToke)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(TEntity entity, bool saveId = false)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(TEntity entity, CancellationToken cancellationToken, bool saveId = false)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool raiseEvent = true)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TEntity entity, bool raiseEvent = true)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(List<TEntity> ents, bool raiseEvent = true)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(IEnumerable<TKey> ids, bool raiseEvent = true)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TKey id, bool raiseEvent = true)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(List<TEntity> ents, CancellationToken cancellationToken, bool raiseEvent = true)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken, bool raiseEvent = true)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken, bool raiseEvent = true)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TKey id, CancellationToken cancellationToken, bool raiseEvent = true)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryShaper, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetAsync<TResult>(Func<IQueryable<TEntity>, TResult> queryShaper, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(List<TEntity> entities, CancellationToken cancellationToke)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
