using Template.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Template.Domain.Entities.Interfaces;

namespace Template.Domain.Repositories
{
  
    public interface IRepository<TEntity, TKey> : IReadOnlyRepository<TEntity,TKey> where TEntity : class, IEntity<TKey>   where TKey:IEquatable<TKey>
    {

        #region Insert
        Task AddAsync(TEntity entity, bool saveId = false);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken, bool saveId = false);       
        Task AddAsync(List<TEntity> entities, CancellationToken cancellationToke);
        #endregion

        #region Update
        Task UpdateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity,CancellationToken cancellationToken);
        Task UpdateAsync(List<TEntity> entities);
        Task UpdateAsync(List<TEntity> entities, CancellationToken cancellationToke);
        #endregion

        #region Delete
        Task DeleteAsync(TKey id,bool raiseEvent=true);
        Task DeleteAsync(TKey id,CancellationToken cancellationToken, bool raiseEvent = true);
        Task DeleteAsync(TEntity entity, bool raiseEvent = true);
        Task DeleteAsync(TEntity entity,CancellationToken cancellationToken, bool raiseEvent = true);
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate,bool raiseEvent=true);
        Task DeleteAsync(List<TEntity> ents, bool raiseEvent = true);
        Task DeleteAsync(IEnumerable<TKey> ids, bool raiseEvent = true);
        Task DeleteAsync(List<TEntity> ents, CancellationToken cancellationToken, bool raiseEvent = true);
        Task DeleteAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken, bool raiseEvent = true);
        Task DeleteAllAsync();
        #endregion

        Task SaveChangesAsync(CancellationToken cancellationToken);

      
    }
}
