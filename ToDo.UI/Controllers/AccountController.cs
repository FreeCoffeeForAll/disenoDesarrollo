using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Application.Components;
using ToDo.Application.Contracts.Identity;
using ToDo.Application.Contracts.Services.Infrastructure;
using ToDo.UI.Models.InputModels;

namespace ToDo.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IGoogleRecaptchaService _recaptchaService;

        public AccountController(IAccountService accountService, IGoogleRecaptchaService recaptchaService)
        {
            _accountService = accountService;
            _recaptchaService = recaptchaService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterInputModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.Register(inputModel.Email, inputModel.Password);
                if (result.IsSuccess)
                {
                    result = await _accountService.Login(inputModel.Email, inputModel.Password);

                    if (result.IsSuccess)
                    {
                        return LocalRedirect("/home/index");
                    }
                }

                ModelState.AddModelError(string.Empty, result.Error);
            }

            return View(inputModel);
        }


        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var model =
                new LoginInputModel { ExternalLogins = await _accountService.GetExternalLogins() };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                var recaptchaResult = await _recaptchaService.VerifyToken(inputModel.Token);
                if (!recaptchaResult)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Recaptcha!");
                }
                else
                {
                    var result = await _accountService.Login(inputModel.Email, inputModel.Password);

                    if (result.IsSuccess)
                    {
                        return LocalRedirect("/home/index");
                    }

                    ModelState.AddModelError(string.Empty, result.Error);
                }
            }

            inputModel.ExternalLogins = await _accountService.GetExternalLogins();
            return View(inputModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            Result result = await _accountService.Logout();
            if (result.IsSuccess)
            {
                return LocalRedirect("~/");
            }

            return StatusCode(500);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl =
                Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _accountService.ConfigureExternalLogin(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        [Route("signin-google")]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> ExternalLoginCallback
            (string returnUrl = null, string remoteError = null)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "~/";
            }

            var model =
                new LoginInputModel
                {
                    ExternalLogins = await _accountService.GetExternalLogins(),
                    ReturnUrl = returnUrl
                };

            if (!string.IsNullOrEmpty(remoteError)) 
            { 
                ModelState.AddModelError(string.Empty, $"Error from external provider: ${remoteError}");
                return View(nameof(Login), model);
            }

            var result = await _accountService.ExternalLogin();
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error);
                return View(nameof(Login), model);
            }

            return LocalRedirect(returnUrl);
        }
    }
}
