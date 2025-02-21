﻿using Library.Application.Interfaces;
using Library.Infrastructure.Data;
using Library.Infrastructure.Extensions;
using Library.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentityExtensions();
            services.AddAuthExtensions();

            services.AddScoped<AppIdentityDbInitialiser>(); 
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IMemberService, MemberService>();
            return services;
        }
    }
}
