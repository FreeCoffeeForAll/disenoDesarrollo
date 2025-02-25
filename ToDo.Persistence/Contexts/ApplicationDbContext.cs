using Microsoft.EntityFrameworkCore;
using ToDo.Application.Contracts.Contexts;
using ToDo.Domain.Entities;
using Task = ToDo.Domain.Entities.Task;

namespace ToDo.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Task> Tasks { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public void Save() 
        {
            this.SaveChanges();
        }
    }
}
