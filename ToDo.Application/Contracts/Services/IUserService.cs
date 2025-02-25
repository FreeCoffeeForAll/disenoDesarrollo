using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domain.DTOs.User;
using ToDo.Domain.Entities;

namespace ToDo.Application.Contracts.Services
{
    public interface IUserService
    {
        IEnumerable<ListUserDTO> GetAll();
    }
}
