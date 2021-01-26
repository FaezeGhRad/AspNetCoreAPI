using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rangle.Abstractions;
using Rangle.Implementations;

namespace Rangle.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public async static Task Initialize(this IApplicationBuilder app)
        {
            using (IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                using (ApplicationDbContext applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    await applicationDbContext.Database.MigrateAsync();

                    IDataInitializer dataInitializer = scope.ServiceProvider.GetRequiredService<IDataInitializer>();

                    await dataInitializer.Initialize();
                }
            }
        }
    }
}
