using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Contracts.Cache;
using ToDo.Application.Contracts.Repositories;
using ToDo.Domain.Entities;

namespace ToDo.Persistence.Contexts.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
