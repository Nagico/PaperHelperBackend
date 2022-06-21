namespace PaperHelper.Entities.Entities;

/// <summary>
/// 论文标签关系
/// </summary>
public class PaperTag
{
    public int Id { get; set; }
    
    public int PaperId { get; set; }
    public virtual Paper? Paper { get; set; }
    
    public int TagId { get; set; }
    public virtual Tag? Tag { get; set; }
}