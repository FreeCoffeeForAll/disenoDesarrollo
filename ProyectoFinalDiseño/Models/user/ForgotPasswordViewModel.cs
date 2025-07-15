using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalDiseño.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
