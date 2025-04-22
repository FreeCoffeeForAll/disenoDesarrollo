public class ExerciseProgress
{
    public int Id { get; set; }

    public int ClientTrainingId { get; set; }
    public ClientTraining ClientTraining { get; set; }

    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; }

    public bool IsCompleted { get; set; }
}
