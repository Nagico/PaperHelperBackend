using Newtonsoft.Json;

namespace PaperHelper.Entities.Entities;

/// <summary>
/// 笔记
/// </summary>
public class Note
{
    public int Id { get; set; }
    
    /// <summary>
    /// 笔记类型 0-文本 1-思维导图 2-批注
    /// </summary>
    public int Type { get; set; }
    /// <summary>
    /// 笔记标题
    /// </summary>
    public string? Title { get; set; }
    /// <summary>
    /// 笔记内容
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// 所属论文
    /// </summary>
    [JsonIgnore]
    public int PaperId { get; set; }
    [JsonIgnore]
    public virtual Paper? Paper { get; set; }
    
    public DateTime CreateTime { get; set; }
    public DateTime? UpdateTime { get; set; }
}