using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalDiseño.Models
{
    public class UserRegistration 
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Username must be at least {2} characters long.", MinimumLength = 3)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Lastname { get; set; }

        public DateTime? DateOfBirth { get; set; }

        private string _profilePicture = "string";
        public string ProfilePicture 
        { 
            get => _profilePicture;
            set => _profilePicture =string.IsNullOrEmpty(value) ? "string" : value;
        }

    }
}
