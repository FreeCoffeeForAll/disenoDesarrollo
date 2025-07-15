public class noUse_TrainingExercise
{
    public int TrainingId { get; set; }
    public noUse_Training Training { get; set; }

    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; }

    public int Order { get; set; } // Optional: control sequence of exercises
}
