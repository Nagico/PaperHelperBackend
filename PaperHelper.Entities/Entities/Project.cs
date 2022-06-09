namespace PaperHelper.Entities.Entities;

public class Project
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public List<UserProject>? Members { get; set; }
    public List<Attachment>? Attachments { get; set; }
    public List<Note>? Notes { get; set; }

    public DateTime CreateTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    
}