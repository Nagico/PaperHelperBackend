using Newtonsoft.Json.Linq;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;

namespace PaperHelper.Services.Serializers;

/// <summary>
/// 附件序列化器
/// </summary>
public class AttachmentSerializer : BaseSerializer
{
    public AttachmentSerializer(PaperHelperContext context) : base(context) {}

    public JObject AttachmentDetail(Attachment attachment)
    {
        var jObject = JObject.FromObject(attachment);
        return jObject;
    }
}