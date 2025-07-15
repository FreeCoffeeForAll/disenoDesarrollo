using Microsoft.AspNetCore.Identity;
using ProyectoFinalDiseño.Models.subscription;
using System;

namespace ProyectoFinalDiseño.Models.user
{
    public class User_Application : IdentityUser
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public DateTime? DateOfBirth { get; set; }

        private string _profilePicture = "string";
        public string ProfilePicture 
        {
            get => _profilePicture;
            set => _profilePicture = string.IsNullOrEmpty(value) ? "string":value; 
        }

        public ICollection<Subscription> Subscriptions { get; set; } // Navigation property to subscriptions
    }
}
