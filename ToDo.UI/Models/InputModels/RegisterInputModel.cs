using System.ComponentModel.DataAnnotations;

namespace ToDo.UI.Models.InputModels
{
    public class RegisterInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Password { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
