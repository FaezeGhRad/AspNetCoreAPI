using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rangle.Abstractions;
using Rangle.Abstractions.Entities;
using Rangle.Abstractions.Services;

namespace Rangle.Implementations
{
    public class DataInitializer : IDataInitializer
    {
        private readonly IAccountService _accountService;

        public DataInitializer(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task Initialize(CancellationToken cancellationToken = default)
        {
            if ((await _accountService.GetUsers(cancellationToken)).Any())
            {
                return;
            }

            UserEntity testUser1 = new UserEntity { Username = "JohnSmith", Password = "Test1234!:|" };
            UserEntity testUser2 = new UserEntity { Username = "User2", Password = "password2" };

            await _accountService.Register(testUser1, cancellationToken);
            await _accountService.Register(testUser2, cancellationToken);
        }
    }
}
