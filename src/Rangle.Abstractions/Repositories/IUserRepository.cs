using System.Threading;
using System.Threading.Tasks;
using Rangle.Abstractions.Entities;

namespace Rangle.Abstractions.Repositories
{
    public interface IUserRepository : IRepository<UserEntity, int>
    {
        Task<UserEntity> GetByUsername(string username, CancellationToken cancellationToken);
    }
}
