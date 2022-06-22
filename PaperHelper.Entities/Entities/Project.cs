using Newtonsoft.Json;

namespace PaperHelper.Entities.Entities;

/// <summary>
/// 项目
/// </summary>
public class Project
{
    public int Id { get; set; }
    /// <summary>
    /// 项目名
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// 项目描述
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// 项目邀请码
    /// </summary>
    public string? InvitationCode { get; set; }
    
    /// <summary>
    /// 项目论文列表
    /// </summary>
    [JsonIgnore]
    public List<Paper>? Papers { get; set; }
    
    /// <summary>
    /// 项目成员列表
    /// </summary>
    [JsonIgnore]
    public List<UserProject>? Members { get; set; }
    
    /// <summary>
    /// 项目笔记列表
    /// </summary>
    [JsonIgnore]
    public List<Note>? Notes { get; set; }

    public DateTime CreateTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    
}