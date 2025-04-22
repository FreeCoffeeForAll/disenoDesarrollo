using System.ComponentModel.DataAnnotations;

public class Training
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    public string SubscriptionLevel { get; set; } // "Basic" or "Premium"

    public ICollection<TrainingExercise> TrainingExercises { get; set; }
}
