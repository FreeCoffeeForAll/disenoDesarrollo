using ToDo.Domain.Entities;
using Task = ToDo.Domain.Entities.Task;

namespace ToDo.Application.Contracts.Repositories
{
    public interface ITaskRepository : IRepository<Task>
    {
    }
}
