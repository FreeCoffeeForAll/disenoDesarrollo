using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalDiseño.Models.user
{
    public class User_Login
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
