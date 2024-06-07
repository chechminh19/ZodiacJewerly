﻿using Application;
using Application.IRepositories;
using Application.IService;
using Application.Services;
using Infrastructure;
using Infrastructure.Repositories;
using System.Diagnostics;

namespace ZodiacJewelryWebApI
{
    public static class DependencyInject
    {
        public static IServiceCollection AddWebAPIService(this IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions (option=> option.JsonSerializerOptions.PropertyNamingPolicy=System.Text.Json.JsonNamingPolicy.KebabCaseLower);
            /*services.AddFluentValidation();*/ 
            
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHealthChecks();
            //services.AddCors();
            /*services.AddSingleton<GlobalExceptionMiddleware>();
            services.AddSingleton<PerformanceMiddleware>();*/
            services.AddSingleton<Stopwatch>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            //services.AddSingleton<ICurrentTime, CurrentTime>();
            //services.AddScoped<IClaimsService, ClaimsService>();

            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<IProductService, ProductService>();
            services.AddHttpContextAccessor();


            services.AddScoped<IZodiacProductRepo, ZodiacProductRepo>();
            services.AddScoped<IZodiacProductService, ZodiacService>();

            services.AddScoped<IOrderRepo, OrderRepo>();
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}
