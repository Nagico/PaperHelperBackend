namespace PaperHelper.Entities.Entities;

/// <summary>
/// 论文引用关系
/// </summary>
public class PaperReference
{
    public int Id { get; set; }
    
    /// <summary>
    /// 引用论文
    /// </summary>
    public int PaperId { get; set; }
    public virtual Paper? Paper { get; set; }
    
    /// <summary>
    /// 被引论文
    /// </summary>
    public int RefPaperId { get; set; }
    public virtual Paper? RefPaper { get; set; }
}