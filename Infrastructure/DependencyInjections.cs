using Application;
using Application.IRepositories;
using Application.IService;
using Application.Services;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddInfrastructuresService(this IServiceCollection services)
        {
            services.AddScoped<IUserRepo, UserRepo>();          
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ICurrentTime, CurrentTime>();
            //services.AddDbContext<AppDbContext>(option => option.UseSqlServer(databaseConnection));
            return services;
        }
    }
}
