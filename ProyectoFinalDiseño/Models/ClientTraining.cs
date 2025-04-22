using ProyectoFinalDiseño.Models;
using System.ComponentModel.DataAnnotations;

public class ClientTraining
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }
    public UserApplication User { get; set; }

    [Required]
    public int TrainingId { get; set; }
    public Training Training { get; set; }

    public DateTime AssignedDate { get; set; }

    public ICollection<ExerciseProgress> ExerciseProgresses { get; set; }
}
