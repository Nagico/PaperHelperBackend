﻿using Newtonsoft.Json;

namespace PaperHelper.Entities.Entities;

/// <summary>
/// 附件
/// </summary>
public class Attachment
{
    public int Id { get; set; }
    
    /// <summary>
    /// 附件类型
    /// </summary>
    public int Type { get; set; }
    
    /// <summary>
    /// 文件名
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// 扩展名
    /// </summary>
    public string? Ext { get; set; }
    
    /// <summary>
    /// 文件链接
    /// </summary>
    public Uri? Url { get; set; }
    
    /// <summary>
    /// MD5摘要
    /// </summary>
    [JsonIgnore]
    public string? Md5 { get; set; }
    
    /// <summary>
    /// DOI
    /// </summary>
    public string? Doi { get; set; }

    public DateTime CreateTime { get; set; }
    public DateTime? UpdateTime { get; set; }
}