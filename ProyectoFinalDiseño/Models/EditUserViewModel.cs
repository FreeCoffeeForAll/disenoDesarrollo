using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalDiseño.Models
{
    public class EditUserViewModel : IdentityUser
    {
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Lastname { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string ProfilePicture { get; set; }

        // For managing roles
        public string SelectedRoles { get; set; }
        public List<string> AllRoles { get; set; }
    }
}
