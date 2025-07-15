using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalDiseño.Models.Training
{
    public class TrainingClassViewModel
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public string StartTime { get; set; } // string para el input type="time"

        [Required]
        [Display(Name = "End Time")]
        public string EndTime { get; set; } // string para el input type="time"

        [Required]
        [Range(1, 100)]
        public int MaxParticipants { get; set; }
    }
}
