using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Rangle.Implementations;
using Xunit;

namespace Rangle.Tests
{
    public abstract class TestBase
    {
        protected internal ApplicationDbContext ApplicationDbContextSqliteInMemory
        {
            get
            {
                var connection = new SqliteConnection($"DataSource=:memory:");
                connection.Open();

                DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(connection)
                    .Options;

                var applicationDbContext = new ApplicationDbContext(dbContextOptions);

                applicationDbContext.Database.EnsureCreated();

                return applicationDbContext;
            }
        }
    }
}
