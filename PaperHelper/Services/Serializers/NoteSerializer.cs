using Newtonsoft.Json.Linq;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;

namespace PaperHelper.Services.Serializers;

/// <summary>
/// Note序列化器
/// </summary>
public class NoteSerializer : BaseSerializer
{
    public NoteSerializer(PaperHelperContext context) : base(context) {}
    
    
    /// <summary>
    /// 思维导图序列化
    /// </summary>
    /// <param name="note">note对象</param>
    /// <returns>思维导图JSON</returns>
    public JObject MindMap(Note note)
    {
        var res = JObject.FromObject(note);
        
        // 处理JSON内容
        res["content"] = JObject.Parse(res["content"].ToString());
        return res;
    }

    /// <summary>
    /// 获取笔记简要信息
    /// </summary>
    /// <param name="note">笔记对象</param>
    /// <returns>笔记JSON</returns>
    public JObject NoteInfo(Note note)
    {
        var res = new JObject
        {
            ["id"] = note.Id,
            ["title"] = note.Title,
            ["update_time"] = note.UpdateTime
        };
        
        return res;
    }
}