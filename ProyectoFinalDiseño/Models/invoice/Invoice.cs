using ProyectoFinalDiseño.Models.user;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalDiseño.Models.invoice
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User_Application User { get; set; }

        public DateTime BillingDate { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public bool IsPaid { get; set; } = false;
    }
}
