using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Contracts.Contexts;
using ToDo.Application.Contracts.Identity;
using ToDo.Application.Contracts.Repositories;
using ToDo.Application.Contracts.Services.Infrastructure;
using ToDo.Domain.Configurations;
using ToDo.Infrastructure.Contexts;
using ToDo.Infrastructure.Identity;
using ToDo.Infrastructure.Services;
using System.Security;
using ToDo.Infrastructure.Contants;
using ToDo.Infrastructure.Authorization.Requirements;
using ToDo.Infrastructure.Models.PocoModels;
using ToDo.Infrastructure.Authorization.Handlers;

namespace ToDo.Infrastructure
{
    public static class Injection
    {
        public static IServiceCollection AddInfrastructureServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"));
            });

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 3;
            }).AddEntityFrameworkStores<ApplicationIdentityDbContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            }).AddGoogle(options =>
            {
                var settings =
                        configuration.GetSection("GoogleAuthentication").Get<GoogleAuthentication>();

                options.ClientId = settings.ClientID;
                options.ClientSecret = settings.ClientSecret;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyType.CreateTask,
                    policy => policy.AddRequirements
                        (new TaskAuthorizationRequirement(new List<TaskPermission>() 
                            { new TaskPermission { Controller = "Home", Action = "Create" } })));
            });

            services.AddScoped<IAuthorizationHandler, TaskAuthorizationHandler>();

            services.AddScoped<IAccountService, AccountService>();

            services.Configure<GoogleRecaptcha>
                (options =>
                {
                    var settings =
                        configuration.GetSection("GoogleRecaptcha").Get<GoogleRecaptcha>();

                    options.SiteKey = settings.SiteKey;
                    options.SecretKey = settings.SecretKey;
                    options.VerifyUrl = settings.VerifyUrl;
                });
            services.AddScoped<IGoogleRecaptchaService, GoogleRecaptchaService>();

            return services;
        }
    }
}
