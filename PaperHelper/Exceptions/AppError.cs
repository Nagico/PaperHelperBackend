using System.Runtime.Serialization;

namespace PaperHelper.Exceptions;

/// <summary>
/// 应用程序异常类
/// </summary>
[DataContract]
public class AppError : ApplicationException
{
    /// <summary>
    /// 错误码
    /// </summary>
    [DataMember]
    public string Code { get; set; }
    /// <summary>
    /// 错误信息
    /// </summary>
    [DataMember]
    public new string? Message { get; set; }
    /// <summary>
    /// 错误描述
    /// </summary>
    [DataMember]
    public string? Detail { get; set; }
    
    public AppError(string code, string? detail = null, string? message=null) : base(message)
    {
        Code = code;
        Detail = detail;
    }
}