using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalDiseño.Models.Training
{
    public class TrainingClass
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public int MaxParticipants { get; set; }

        public int CurrentParticipants { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<ClassReservation> Reservations { get; set; }
    }
}
