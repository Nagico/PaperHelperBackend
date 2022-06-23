
using Newtonsoft.Json;

namespace PaperHelper.Entities.Entities;

/// <summary>
/// 标签
/// </summary>
public class Tag
{
    public int Id { get; set; }

    /// <summary>
    /// 标签内容
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// 包含的论文
    /// </summary>
    [JsonIgnore]
    public List<PaperTag>? Papers { get; set; }
}