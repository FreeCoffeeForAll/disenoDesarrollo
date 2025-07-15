using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalDiseño.Models.user
{
    public class User_Visit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User_Application User { get; set; }

        public DateTime VisitTime { get; set; } = DateTime.Now;
    }
}
