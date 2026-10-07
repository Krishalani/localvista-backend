namespace LocalVista.Data.Entities;

public class AttractionImage
{
    public int AttractionImageId { get; set; }
    public int AttractionId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int SortOrder { get; set; }

    public Attraction Attraction { get; set; } = null!;
}
