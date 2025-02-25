using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Entities;
using Task = ToDo.Domain.Entities.Task;

namespace ToDo.Application.Contracts.Contexts
{
    public interface IApplicationDbContext
    {
        DbSet<Task> Tasks { get; set; }

        DbSet<User> Users { get; set; }

        void Save();
    }
}
