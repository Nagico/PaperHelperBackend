namespace PaperHelper.Entities.Entities;

/// <summary>
/// 论文
/// </summary>
public class Paper
{
    public int Id { get; set; }
    
    /// <summary>
    /// 所属项目
    /// </summary>
    public int ProjectId { get; set; }
    public virtual Project Project { get; set; }
    
    
    /// <summary>
    /// 论文标题
    /// </summary>
    public string? Title { get; set; }
    /// <summary>
    /// 摘要
    /// </summary>
    public string? Abstract { get; set; }
    /// <summary>
    /// 关键字 json列表
    /// </summary>
    public string? Keywords { get; set; }
    /// <summary>
    /// 作者 json列表
    /// </summary>
    public string? Authors { get; set; }
    /// <summary>
    /// 出版单位
    /// </summary>
    public string? Publication { get; set; }
    /// <summary>
    /// 卷号
    /// </summary>
    public string? Volume { get; set; }
    /// <summary>
    /// 页号
    /// </summary>
    public string? Pages { get; set; }
    
    /// <summary>
    /// 出版日期
    /// </summary>
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
    
    /// <summary>
    /// 论文url
    /// </summary>
    public Uri? Url { get; set; }
    /// <summary>
    /// 论文doi
    /// </summary>
    public string? Doi { get; set; }
    
    /// <summary>
    /// 附件列表
    /// </summary>
    public List<Attachment>? Attachments { get; set; }
    /// <summary>
    /// 笔记列表
    /// </summary>
    public List<Note>? Notes { get; set; }
    /// <summary>
    /// 标签列表
    /// </summary>
    public List<PaperTag>? Tags { get; set; }
    /// <summary>
    /// 参考文献列表
    /// </summary>
    public List<PaperReference>? References { get; set; }
    /// <summary>
    /// 被引用列表
    /// </summary>
    public List<PaperReference>? ReferenceFrom { get; set; }

    public DateTime? CreateTime { get; set; }
    public DateTime? UpdateTime { get; set; }
}