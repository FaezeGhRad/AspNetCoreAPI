using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rangle.Abstractions.Entities;

namespace Rangle.Abstractions.Repositories
{
    public interface IEntityRepository : IRepository<Entity, Guid>
    {
        /// <summary>
        /// Get entities keys
        /// </summary>
        /// <param name="cancellationToken">cancellationToken</param>
        Task<IEnumerable<Guid>> GetKeys(CancellationToken cancellationToken = default);
    }
}
