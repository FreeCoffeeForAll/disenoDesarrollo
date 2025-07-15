using ProyectoFinalDiseño.Models.user;
using System.ComponentModel.DataAnnotations;

public class noUse_Training_Client
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }
    public User_Application User { get; set; }

    [Required]
    public int TrainingId { get; set; }
    public noUse_Training Training { get; set; }

    public DateTime AssignedDate { get; set; }

    public ICollection<noUse_Training_Progress> ExerciseProgresses { get; set; }
}
