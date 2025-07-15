using System.ComponentModel.DataAnnotations;

public class Exercise
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    public string VideoUrl { get; set; } // Optional instructional video

    public string SubscriptionLevel { get; set; } // "Basic" or "Premium"
}
