using ToDo.Application.Components;
using ToDo.Domain.DTOs.Task;
using Task = ToDo.Domain.Entities.Task;

namespace ToDo.Application.Contracts.Services
{
    public interface ITaskService
    {
        EditTaskDTO Get(int id);

        IEnumerable<ListTaskDTO> GetAll();

        Result<int> Create(CreateTaskDTO dto);

        Result Edit(EditTaskDTO dto);

        Result Delete(int id);
    }
}
