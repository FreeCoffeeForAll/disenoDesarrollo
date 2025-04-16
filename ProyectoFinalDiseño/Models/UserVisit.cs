using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalDiseño.Models
{
    public class UserVisit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserApplication User { get; set; }

        public DateTime VisitTime { get; set; } = DateTime.Now;
    }
}
