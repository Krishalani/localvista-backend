namespace LocalVista.Data.Entities;

public class Attraction
{
    public int AttractionId { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? OpeningHours { get; set; }
    public string? TravelTips { get; set; }
    public string BestVisitMonths { get; set; } = string.Empty;
    public decimal DistanceKm { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public Category Category { get; set; } = null!;
    public ICollection<AttractionImage> Images { get; set; } = new List<AttractionImage>();
    public ICollection<AttractionFeedback> Feedback { get; set; } = new List<AttractionFeedback>();
}
