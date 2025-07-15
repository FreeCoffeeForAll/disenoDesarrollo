using ProyectoFinalDiseño.Models.user;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalDiseño.Models.Training
{
    public class ClassReservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User_Application User { get; set; }

        [Required]
        public int TrainingClassId { get; set; }

        [ForeignKey("TrainingClassId")]
        public virtual TrainingClass TrainingClass { get; set; }

        public DateTime ReservationDate { get; set; } = DateTime.Now;
    }
}
