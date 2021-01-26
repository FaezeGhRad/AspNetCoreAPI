using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Rangle.Abstractions.Entities;
using Rangle.Abstractions.Exceptions;
using Rangle.Abstractions.Repositories;
using Rangle.Abstractions.Services;

namespace Rangle.Implementations.Repositories
{
    public class EntityRepository : IEntityRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDataProtectionService _dataProtection;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string _privateKey = null;
        public virtual string PrivateKey
        {
            get
            {
                if (string.IsNullOrEmpty(_privateKey))
                {
                    _privateKey = _httpContextAccessor?.HttpContext?.User?.FindFirst(Constants.DATA_PROTECTION_KEY_NAME)?.Value;
                }

                return _privateKey;
            }
        }

        private int _currentUserId = 0;
        public virtual int CurrentUserId
        {
            get
            {
                if (_currentUserId == 0)
                {
                    string userId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    _currentUserId = string.IsNullOrEmpty(userId) ? 0 : int.Parse(userId);
                }

                return _currentUserId;
            }
        }

        public EntityRepository(ApplicationDbContext dbContext, IDataProtectionService dataProtection, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _dataProtection = dataProtection;
            _httpContextAccessor = httpContextAccessor;
        }

        ///<inheritdoc/>
        public Task<IEnumerable<Guid>> GetKeys(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_dbContext.Entities
                .AsNoTracking()
                .Where(e => e.CreatorId == CurrentUserId)
                .Select(e => e.Id)
                .AsEnumerable());
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<Entity>> Get(CancellationToken cancellationToken = default)
        {
            List<Entity> entities = await _dbContext.Entities.AsNoTracking()
                .Where(e => e.CreatorId == CurrentUserId)
                .ToListAsync();

            entities.ForEach(e => e.JsonObject = _dataProtection.Decrypt(e.JsonObject, PrivateKey));
            return entities;
        }

        ///<inheritdoc/>
        public async Task<Entity> Get(Guid key, CancellationToken cancellationToken = default)
        {
            Entity entity = await _dbContext.Entities.AsNoTracking().SingleOrDefaultAsync(e => e.Id == key);

            if (CurrentUserId != entity.CreatorId)
            {
                throw new UnauthorizedException("The current user doesn't have enough permissions to access this resource!");
            }

            entity.JsonObject = _dataProtection.Decrypt(entity.JsonObject, PrivateKey);
            return entity;
        }

        ///<inheritdoc/>
        public async Task<Entity> Create(Entity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.JsonObject = _dataProtection.Encrypt(entity.JsonObject, PrivateKey);
            entity.CreatorId = CurrentUserId;

            _dbContext.Entities.Add(entity);
            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        ///<inheritdoc/>
        public async Task<Entity> Update(Entity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity.Id == null || entity.Id == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(entity.Id)} is null!");
            }

            Entity currentEntity = await _dbContext.Entities.FindAsync(entity.Id)
                ?? throw new EntityNotFoundException(entity.Id.ToString());

            if (CurrentUserId != currentEntity.CreatorId)
            {
                throw new UnauthorizedException("The current user doesn't have enough permissions to modify this resource!");
            }

            currentEntity.JsonObject = _dataProtection.Encrypt(entity.JsonObject, PrivateKey);

            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(currentEntity).State = EntityState.Detached;
            return currentEntity;
        }

        ///<inheritdoc/>
        public async Task Delete(Guid key, CancellationToken cancellationToken = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            Entity currentEntity = await _dbContext.Entities.FindAsync(key)
                ?? throw new EntityNotFoundException(key.ToString());

            if (CurrentUserId != currentEntity.CreatorId)
            {
                throw new UnauthorizedException("The current user doesn't have enough permissions to remove this resource!");
            }

            _dbContext.Remove(currentEntity);
            await _dbContext.SaveChangesAsync();
        }
    }
}