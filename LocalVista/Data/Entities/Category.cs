namespace LocalVista.Data.Entities;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<Attraction> Attractions { get; set; } = new List<Attraction>();
}
