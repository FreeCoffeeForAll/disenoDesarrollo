using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Components;

namespace ToDo.Application.Contracts.Identity
{
    public interface IAccountService
    {
        Task<Result> Register(string email, string password);

        Task<Result> Login(string email, string password);

        Task<Result> Logout();

        Task<IEnumerable<AuthenticationScheme>> GetExternalLogins();

        AuthenticationProperties ConfigureExternalLogin(string provider, string redirectUrl);

        Task<Result> ExternalLogin();

        Result HasAccess(string email, string controller, string action);
    }
}
