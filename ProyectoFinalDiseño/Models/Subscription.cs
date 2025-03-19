using ProyectoFinalDiseño.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourApp.Models
{
    public class Subscription
    {
        public int Id { get; set; } // Primary Key for Subscription

        [Required]
        public string UserId { get; set; } // Foreign Key to IdentityUser

        [ForeignKey("UserId")]
        public virtual UserApplication UserApplication { get; set; } // Navigation property to the ApplicationUser (if needed)

        public DateTime StartDate { get; set; } // Subscription start date

        public DateTime? EndDate { get; set; } // Optional subscription end date

        public string Plan { get; set; } // E.g., Basic, Premium, etc.

        public bool IsActive => EndDate == null || EndDate > DateTime.Now; // Check if the subscription is active
    }
}
