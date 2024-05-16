using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddInfrastructuresService(this IServiceCollection services, string databaseConnection)
        {
            services.AddDbContext<AppDbContext>(option => option.UseSqlServer(databaseConnection));
            return services;
        }
    }
}
