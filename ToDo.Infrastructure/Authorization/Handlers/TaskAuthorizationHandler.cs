using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ToDo.Application.Contracts.Identity;
using ToDo.Infrastructure.Authorization.Requirements;
using ToDo.Infrastructure.Extensions;

namespace ToDo.Infrastructure.Authorization.Handlers
{
    public class TaskAuthorizationHandler : AuthorizationHandler<TaskAuthorizationRequirement>, IAuthorizationHandler
    {
        private readonly IAccountService _accountService;

        public TaskAuthorizationHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TaskAuthorizationRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
            }
            else
            {
                var access = false;

                foreach (var permission in requirement.Permissions)
                {
                    access = access ||
                        _accountService.HasAccess
                            (context.User.Identity.GetClaim(ClaimTypes.Email), permission.Controller, permission.Action).IsSuccess;
                }

                if (access)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
