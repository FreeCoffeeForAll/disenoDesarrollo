using Task = ToDo.Domain.Entities.Task;
using ToDo.Application.Contracts.Repositories;
using ToDo.Application.Contracts.Cache;

namespace ToDo.Persistence.Contexts.Repositories
{
    public class TaskRepository : Repository<Task>, ITaskRepository
    {
        public TaskRepository(ApplicationDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
