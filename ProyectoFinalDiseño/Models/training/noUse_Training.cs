using System.ComponentModel.DataAnnotations;

public class noUse_Training
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    public string SubscriptionLevel { get; set; } // "Basic" or "Premium"

    public ICollection<noUse_TrainingExercise> TrainingExercises { get; set; }
}
