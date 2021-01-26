using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rangle.Abstractions.Entities;

namespace Rangle.Abstractions.Services
{
    public interface IAccountService
    {
        Task<UserEntity> Authenticate(UserEntity user, CancellationToken cancellationToken);

        Task<UserEntity> Register(UserEntity user, CancellationToken cancellationToken);

        Task<IEnumerable<UserEntity>> GetUsers(CancellationToken cancellationToken);

        Task<UserEntity> GetUser(int userId, CancellationToken cancellationToken);
    }
}
