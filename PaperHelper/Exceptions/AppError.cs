using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PaperHelper.Exceptions;

[DataContract]
public class AppError : ApplicationException
{
    [DataMember]
    public string Code { get; set; }
    [DataMember]
    public new string? Message { get; set; }
    [DataMember]
    public string? Detail { get; set; }
    
    public AppError(string code, string? detail = null, string? message=null) : base(message)
    {
        Code = code;
        Detail = detail;
    }
}