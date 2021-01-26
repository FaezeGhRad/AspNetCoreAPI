using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rangle.Abstractions.Entities;

namespace Rangle.Abstractions.Repositories
{
    public interface IRepository<TEntity, TKey> where TEntity : EntityBase<TKey>
    {
        /// <summary>
        /// Get entities
        /// </summary>
        /// <param name="cancellationToken">cancellationToken</param>
        Task<IEnumerable<TEntity>> Get(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get an entity based on the given key
        /// </summary>
        /// <param name="key">the key of entity</param>
        /// <param name="cancellationToken">cancellationToken</param>
        Task<TEntity> Get(TKey key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create an entity
        /// </summary>
        /// <param name="entity">entity to create</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns></returns>
        Task<TEntity> Create(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="entity">entity to update</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns></returns>
        Task<TEntity> Update(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="key">the key of entity</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns></returns>
        Task Delete(TKey key, CancellationToken cancellationToken = default);
    }
}
