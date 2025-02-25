using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Contracts.Repositories;
using ToDo.Application.Contracts.Services;
using ToDo.Domain.DTOs.User;
using ToDo.Domain.Entities;

namespace ToDo.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<ListUserDTO> GetAll()
        {
            var users = _repository.GetAll().ToList();
            return users.ConvertAll(s => new ListUserDTO(s.Id, s.Name));
        }
    }
}
