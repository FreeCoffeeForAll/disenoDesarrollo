using ProyectoFinalDiseño.Models.user;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalDiseño.Models.subscription
{
    public class Subscription
    {
        public int Id { get; set; } // Primary Key for Subscription

        [Required]
        public string UserId { get; set; } // Foreign Key to IdentityUser

        [ForeignKey("UserId")]
        public virtual User_Application UserApplication { get; set; } // Navigation property to the ApplicationUser (if needed)

        public DateTime StartDate { get; set; } // Subscription start date

        public DateTime? EndDate { get; set; } // Optional subscription end date

        public string Plan { get; set; } // E.g., Basic, Premium, etc.

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Price { get; set; } // Price of the subscription

        public bool IsActive => EndDate == null || EndDate > DateTime.Now; // Check if the subscription is active
    }
}
