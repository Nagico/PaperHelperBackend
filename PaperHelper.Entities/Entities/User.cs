using Newtonsoft.Json;

namespace PaperHelper.Entities.Entities;

/// <summary>
/// 用户
/// </summary>
public class User
{
    public int Id { get; set; }
    /// <summary>
    /// 用户名
    /// </summary>
    public string? Username { get; set; }
    /// <summary>
    /// 密码
    /// </summary>
    [JsonIgnore]
    public string? Password { get; set; }
    /// <summary>
    /// 手机号
    /// </summary>
    public string? Phone { get; set; }
    
    /// <summary>
    /// 头像
    /// </summary>
    public Uri? Avatar { get; set; }
    
    /// <summary>
    /// 用户参与项目列表
    /// </summary>
    [JsonIgnore]
    public List<UserProject>? UserProjects { get; set; }

    public DateTime LastLogin { get; set; }
    public DateTime CreateTime { get; set; }
}