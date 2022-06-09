namespace PaperHelper.Entities.Entities;

public class Tag
{
    public int Id { get; set; }

    public string? Name { get; set; }
    
    public List<PaperTag>? Papers { get; set; }
}