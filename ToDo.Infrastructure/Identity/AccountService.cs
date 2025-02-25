using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Components;
using ToDo.Application.Contracts.Identity;
using ToDo.Application.Contracts.Repositories;

namespace ToDo.Infrastructure.Identity
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUserRepository _userRepository;

        public AccountService(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
        }

        public async Task<Result> Register(string email, string password)
        {
            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    builder.AppendLine(error.Description);
                }
                return Result.Fail(builder.ToString());
            }

            return Result.Ok();
        }

        public async Task<Result> Login(string email, string password)
        {
            var result =
                await _signInManager.PasswordSignInAsync(email, password, false, false);

            if (!result.Succeeded)
            {
                return Result.Fail("Invalid user and password combination.");
            }

            return Result.Ok();
        }

        public async Task<Result> Logout()
        {
            await _signInManager.SignOutAsync();
            return Result.Ok();
        }

        public async Task<IEnumerable<AuthenticationScheme>> GetExternalLogins()
        {
            return await _signInManager.GetExternalAuthenticationSchemesAsync();
        }

        public AuthenticationProperties ConfigureExternalLogin(string provider, string redirectUrl)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }

        public async Task<Result> ExternalLogin()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
            {
                return Result.Fail("Invalid user and password combination.");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new IdentityUser { UserName = email, Email = email };
                await _userManager.CreateAsync(user);
            }

            var login = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (login == null)
            {
                await _userManager.AddLoginAsync(user, info);
            }

            var result =
                await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

            if (!result.Succeeded)
            {
                return Result.Fail("Invalid user and password combination.");
            }

            await _signInManager.SignInAsync(user, false);

            return Result.Ok();
        }

        public Result HasAccess(string email, string controller, string action)
        {
            var user =
                _userRepository.Get
                    (s => s.Email == email, includes: i => i.Permissions);
            return
                user == null || !user.Permissions.Any(s => s.Controller.Equals(controller, StringComparison.OrdinalIgnoreCase) && s.Action.Equals(action, StringComparison.OrdinalIgnoreCase))
                    ? Result.Fail(string.Empty)
                    : Result.Ok();
        }
    }
}
