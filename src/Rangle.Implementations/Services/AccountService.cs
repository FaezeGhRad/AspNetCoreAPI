using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rangle.Abstractions.Entities;
using Rangle.Abstractions.Repositories;
using Rangle.Abstractions.Services;

namespace Rangle.Implementations.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _repository;
        private readonly IDataProtectionService _dataProtection;

        public AccountService(IUserRepository repository, IDataProtectionService dataProtectionService)
        {
            _repository = repository;
            _dataProtection = dataProtectionService;
        }

        public async Task<IEnumerable<UserEntity>> GetUsers(CancellationToken cancellationToken)
        {
            IEnumerable<UserEntity> users = await _repository.Get(cancellationToken);

            foreach (UserEntity user in users)
            {
                user.Password = null;
            }

            return users;
        }

        public async Task<UserEntity> GetUser(int userId, CancellationToken cancellationToken)
        {
            UserEntity user = await _repository.Get(userId, cancellationToken);

            if (user == null)
                return null;

            user.Password = null;
            return user;
        }

        public async Task<UserEntity> Register(UserEntity user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(user.Username))
            {
                throw new ArgumentException("Username is null!");
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                throw new ArgumentException("Password is null!");
            }

            if ((await _repository.GetByUsername(user.Username, cancellationToken)) != null)
            {
                throw new ArgumentException($"Username already exists : '{user.Username}'");
            }

            UserEntity newUser = new UserEntity
            {
                Username = user.Username,
                Password = _dataProtection.HashPassword(user.Password)
            };

            await _repository.Create(newUser, cancellationToken);

            newUser.Password = null;

            return newUser;
        }

        public async Task<UserEntity> Authenticate(UserEntity user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(user.Username))
            {
                throw new ArgumentException("Username is null!");
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                throw new ArgumentException("Password is null!");
            }

            UserEntity userEntity = await _repository.GetByUsername(user.Username, cancellationToken);

            if (userEntity == null || !_dataProtection.VerifyPassword(user.Password, userEntity.Password))
            {
                // authentication failed
                return null;
            }

            return userEntity;
        }
    }
}
