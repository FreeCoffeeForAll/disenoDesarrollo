using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Contracts.Contexts;
using Task = ToDo.Domain.Entities.Task;
using ToDo.Persistence.Contexts;
using ToDo.Persistence.Contexts.Repositories;
using ToDo.Application.Contracts.Repositories;

namespace ToDo.Persistence
{
    public static class Injection
    {
        public static IServiceCollection AddPersistenceServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(configuration.GetConnectionString("Default")));

            services.AddScoped<IApplicationDbContext>
                (options => options.GetService<ApplicationDbContext>());

            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
