namespace LocalVista.Data.Entities;

public class AttractionFeedback
{
    public int AttractionFeedbackId { get; set; }
    public int AttractionId { get; set; }
    public string? DisplayName { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public Attraction Attraction { get; set; } = null!;
}
