namespace PaperHelper.Entities.Entities;

/// <summary>
/// 用户项目关系
/// </summary>
public class UserProject
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public virtual User? User { get; set; }
    
    public int ProjectId { get; set; }
    public virtual Project? Project { get; set; }
    
    /// <summary>
    /// 是否为所有者
    /// </summary>
    public bool IsOwner { get; set; }
    
    /// <summary>
    /// 最后访问时间
    /// </summary>
    public DateTime? AccessTime { get; set; }
    /// <summary>
    /// 最后编辑时间
    /// </summary>
    public DateTime? EditTime { get; set; }
    public DateTime? CreateTime { get; set; }
}