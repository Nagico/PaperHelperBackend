namespace PaperHelper.Entities.Entities;

public class Paper
{
    public int Id { get; set; }
    
    public string? Title { get; set; }
    public string? Abstract { get; set; }
    public string? Keywords { get; set; }
    public string? Authors { get; set; }
    public string? Publication { get; set; }
    public string? Volume { get; set; }
    public string? Pages { get; set; }
    
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
    
    public Uri? Url { get; set; }
    public string? Doi { get; set; }
    
    public List<Attachment>? Attachments { get; set; }
    public List<Note>? Notes { get; set; }
    public List<PaperTag>? Tags { get; set; }
    public List<PaperReference>? References { get; set; }
    public List<PaperReference>? ReferenceFrom { get; set; }

    public DateTime? CreateTime { get; set; }
    public DateTime? UpdateTime { get; set; }
}