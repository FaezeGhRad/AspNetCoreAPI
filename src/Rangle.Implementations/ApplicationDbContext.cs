using Microsoft.EntityFrameworkCore;
using Rangle.Abstractions.Entities;

namespace Rangle.Implementations
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Entity> Entities { get; set; }
        public DbSet<UserEntity> Users { get; set; }
    }
}
