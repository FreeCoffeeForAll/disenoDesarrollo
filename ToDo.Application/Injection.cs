using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDo.Application.Cache;
using ToDo.Application.Contracts.Cache;
using ToDo.Application.Contracts.Services;
using ToDo.Application.Services;

namespace ToDo.Application
{
    public static class Injection
    {
        public static IServiceCollection AddApplicationServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICacheStore, CacheStore>();

            return services;
        }
    }
}
