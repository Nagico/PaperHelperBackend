namespace PaperHelper.Entities.Entities;

public class Note
{
    public int Id { get; set; }
    
    
    public int Type { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    
    public int ProjectId { get; set; }
    public virtual Project? Project { get; set; }
    
    public int PaperId { get; set; }
    public virtual Paper? Paper { get; set; }
    
    public DateTime CreateTime { get; set; }
    public DateTime? UpdateTime { get; set; }
}