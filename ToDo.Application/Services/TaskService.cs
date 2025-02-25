using ToDo.Application.Contracts.Services;
using Task = ToDo.Domain.Entities.Task;
using ToDo.Domain.DTOs.Task;
using ToDo.Application.Components;
using ToDo.Application.Contracts.Repositories;
using ToDo.Application.Contracts.Contexts;
using ToDo.Domain.Entities.ValueObjects;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System;
using ToDo.Application.Cache;
using ToDo.Application.Contracts.Cache;

namespace ToDo.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;
        private readonly ICacheStore _cacheStore;

        public TaskService(ITaskRepository repository, ICacheStore cacheStore)
        {
            _repository = repository;
            _cacheStore = cacheStore;
        }

        public EditTaskDTO Get(int id)
        {
            Task task = _repository.Get(s => s.Id == id);
            return new EditTaskDTO(task.Id, task.Subject, task.Effort, task.Completed);
        }

        public IEnumerable<ListTaskDTO> GetAll()
        {
            CacheKey<List<Task>> cacheKey = new CacheKey<List<Task>>();
            List<Task> tasks = _cacheStore.Get(cacheKey);

            if (tasks == null)
            {
                tasks =
                    _repository.GetAll
                       (s => !s.Completed || s.CreationDate.Date >= DateTime.Now.Date.AddMonths(-1),
                           includes: i => i.User).ToList();

                _cacheStore.Add(tasks, cacheKey);
            }

            return
                tasks.ConvertAll
                    (s => new ListTaskDTO(s.Id, s.Subject, s.User.Name, s.Effort.ToString(), s.Completed));
        }

        public Result<int> Create(CreateTaskDTO dto)
        {
            Task task =
                Task.Create
                    (
                        dto.Subject,
                        dto.UserId,
                        new Effort(dto.Weeks, dto.Days, dto.Hours, dto.Minutes)
                    );

            try
            {
                _repository.Insert(task);
                _repository.Save();
            }
            catch
            {
                return Result.Fail<int>("Internal server error.");
            }

            return Result.Ok<int>(task.Id);
        }

        public Result Edit(EditTaskDTO dto)
        {
            throw new NotImplementedException();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
