using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;
using ToDo.UI.Attributes;

namespace ToDo.UI.Models.InputModels
{
    public class LoginInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Password { get; set; }

        [Required]
        public string Token { get; set; }

        [NotRequired]
        public IEnumerable<AuthenticationScheme> ExternalLogins { get; set; }

        [NotRequired]
        public string ReturnUrl { get; set; }
    }
}
