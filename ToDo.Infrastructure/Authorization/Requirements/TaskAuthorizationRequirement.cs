using Microsoft.AspNetCore.Authorization;
using ToDo.Infrastructure.Models.PocoModels;

namespace ToDo.Infrastructure.Authorization.Requirements
{
    public class TaskAuthorizationRequirement : IAuthorizationRequirement
    {
        private List<TaskPermission> _permissions;

        public IReadOnlyList<TaskPermission> Permissions 
        { 
            get { return _permissions; } 
        }

        public TaskAuthorizationRequirement(List<TaskPermission> permissions) 
        {
            _permissions = permissions;
        }
    }
}
