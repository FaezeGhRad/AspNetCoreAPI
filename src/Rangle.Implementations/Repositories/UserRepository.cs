using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rangle.Abstractions.Entities;
using Rangle.Abstractions.Repositories;
using Rangle.Abstractions.Services;

namespace Rangle.Implementations.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext, IDataProtectionService dataProtection)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<UserEntity>> Get(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.AsNoTracking().ToListAsync();
        }

        public async Task<UserEntity> Get(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserEntity> GetByUsername(string username, CancellationToken cancellationToken)
        {
            return await _dbContext.Users.AsNoTracking()
                .SingleOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<UserEntity> Create(UserEntity user, CancellationToken cancellationToken = default)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _dbContext.Users.Add(user);

            await _dbContext.SaveChangesAsync(cancellationToken);
            _dbContext.Entry(user).State = EntityState.Detached;
            return user;
        }

        public Task<UserEntity> Update(UserEntity entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}