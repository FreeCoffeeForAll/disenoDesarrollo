public class noUse_Training_Progress
{
    public int Id { get; set; }

    public int ClientTrainingId { get; set; }
    public noUse_Training_Client ClientTraining { get; set; }

    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; }

    public bool IsCompleted { get; set; }
}
