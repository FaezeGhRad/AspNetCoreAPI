using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Moq;
using Rangle.Abstractions.Entities;
using Rangle.Abstractions.Repositories;
using Rangle.Abstractions.Services;
using Rangle.Implementations;
using Rangle.Implementations.Repositories;
using Rangle.Implementations.Services;
using Xunit;

namespace Rangle.Tests.Services
{
    public class AccountServiceTests : TestBase
    {
        private List<UserEntity> _users = new List<UserEntity> {
            new UserEntity{ Id =1 , Username="username@email.com", Password="Test_123#456_"},
            new UserEntity{ Id =2 , Username="username2@email2.com", Password="PKFFF123#456_&*"},
            new UserEntity{ Id =3 , Username="username3@email3.com", Password="Pass123#456!@"}
        };

        [Fact]
        public async void Register_Success()
        {
            // Arrange
            ApplicationDbContext dbContext = GetApplicationDbContext();
            IDataProtectionService dataProtectionService = new DataProtectionService();
            IUserRepository userRepository = new UserRepository(dbContext, dataProtectionService);
            AccountService service = new AccountService(userRepository, dataProtectionService);

            // Act
            UserEntity user = await service.Register(_users[1], CancellationToken.None);

            // Assert
            Assert.NotNull(user);
            Assert.Null(user.Password);
            Assert.NotEqual(_users[1].Password, dbContext.Users.First().Password);
        }

        [Fact]
        public async void Authenticate_Success()
        {
            // Arrange
            ApplicationDbContext dbContext = GetApplicationDbContext(_users[1]);
            IDataProtectionService dataProtectionService = new DataProtectionService();
            IUserRepository userRepository = new UserRepository(dbContext, dataProtectionService);
            AccountService service = new AccountService(userRepository, dataProtectionService);

            // Act
            UserEntity user = await service.Authenticate(_users[1], CancellationToken.None);

            // Assert
            Assert.NotNull(user);
            Assert.NotNull(user.Password);
        }

        [Fact]
        public async void Authenticate_WrongUsername_Fail()
        {
            // Arrange
            ApplicationDbContext dbContext = GetApplicationDbContext(_users[1]);
            IDataProtectionService dataProtectionService = new DataProtectionService();
            IUserRepository userRepository = new UserRepository(dbContext, dataProtectionService);
            AccountService service = new AccountService(userRepository, dataProtectionService);
            _users[1].Username = _users[1].Username.Substring(1);

            // Act
            UserEntity user = await service.Authenticate(_users[1], CancellationToken.None);

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public async void Authenticate_WrongPassword_Fail()
        {
            // Arrange
            ApplicationDbContext dbContext = GetApplicationDbContext(_users[1]);
            IDataProtectionService dataProtectionService = new DataProtectionService();
            IUserRepository userRepository = new UserRepository(dbContext, dataProtectionService);
            AccountService service = new AccountService(userRepository, dataProtectionService);
            _users[1].Password = _users[1].Password.Substring(1);

            // Act
            UserEntity user = await service.Authenticate(_users[1], CancellationToken.None);

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public async void GetUsers_Success()
        {
            // Arrange
            ApplicationDbContext dbContext = GetApplicationDbContext(_users.ToArray());
            IDataProtectionService dataProtectionService = new DataProtectionService();
            IUserRepository userRepository = new UserRepository(dbContext, dataProtectionService);
            AccountService service = new AccountService(userRepository, dataProtectionService);

            // Act
            IEnumerable<UserEntity> users = await service.GetUsers(CancellationToken.None);

            // Assert
            Assert.NotNull(users);
            Assert.Equal(_users.Count, users.Count());
            foreach (UserEntity user in users)
            {
                Assert.Null(user.Password);
            }
        }

        [Fact]
        public async void GetUser_Success()
        {
            // Arrange
            ApplicationDbContext dbContext = GetApplicationDbContext(_users[0]);
            IDataProtectionService dataProtectionService = new DataProtectionService();
            IUserRepository userRepository = new UserRepository(dbContext, dataProtectionService);
            AccountService service = new AccountService(userRepository, dataProtectionService);

            // Act
            UserEntity user = await service.GetUser(_users[0].Id, CancellationToken.None);

            // Assert
            Assert.NotNull(user);
            Assert.Null(user.Password);
        }

        private ApplicationDbContext GetApplicationDbContext(params UserEntity[] users)
        {
            ApplicationDbContext dbContext = ApplicationDbContextSqliteInMemory;
            if (users != null)
            {
                DataProtectionService dataProtectionService = new DataProtectionService();
                foreach (UserEntity user in users)
                {
                    UserEntity newUser = new UserEntity { Id = user.Id, Username = user.Username };
                    newUser.Password = dataProtectionService.HashPassword(user.Password);
                    dbContext.Users.AddRange(newUser);
                }

                dbContext.SaveChanges();
            }
            return dbContext;
        }
    }
}
