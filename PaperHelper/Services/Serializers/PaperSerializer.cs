using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;

namespace PaperHelper.Services.Serializers;

/// <summary>
/// 论文序列化器
/// </summary>
public class PaperSerializer : BaseSerializer
{
    public PaperSerializer(PaperHelperContext context) : base(context) {}
    
    /// <summary>
    /// 获取论文简要信息
    /// </summary>
    /// <param name="paper">论文对象</param>
    /// <returns>JSON对象</returns>
    public JObject PaperInfo(Paper paper)
    {
        // 获取Tag信息
        var tags = new JArray();
        foreach (var tagInfo in paper.Tags.Select(tag => _context.Tags.Find(tag.TagId)).Where(tagInfo => tagInfo != null))
        {
            tags.Add(JObject.FromObject(tagInfo));
        }
        
        var paperInfo = new JObject
        {
            ["id"] = paper.Id,
            ["title"] = paper.Title,
            ["tag"] = tags,
            ["author"] = JArray.Parse(paper.Authors),
            ["year"] = paper.Year,
            ["publication"] = paper.Publication,
            ["create_time"] = paper.CreateTime,
            ["read"] = true
        };
        return paperInfo;
    }
    
    /// <summary>
    /// 论文详细信息
    /// </summary>
    /// <param name="paper">论文对象</param>
    /// <returns>JSON对象</returns>
    public JObject PaperDetail(Paper paper)
    {
        var paperDetail = JObject.FromObject(paper);
        
        // 处理JSON信息
        paperDetail["keyword"] = JArray.Parse(paperDetail["keywords"].ToString());
        paperDetail["author"] = JArray.Parse(paperDetail["authors"].ToString());
        paperDetail.Remove("keywords");
        paperDetail.Remove("authors");
        
        // 移除无用信息
        paperDetail.Remove("attachment_id");
        paperDetail.Remove("project");
        
        
        // 添加引用信息
        var references = new JArray();
        foreach (var reference in paper.References)
        {
            var refPaper = _context.Papers.Include("Tags").FirstOrDefault(x => x.Id == reference.PaperId);
            if (refPaper != null)
            {
                references.Add(PaperInfo(refPaper));
            }
        }

        paperDetail["reference"] = references;
        paperDetail.Remove("references");
        
        // 添加被引用信息
        var referenceFrom = new JArray();
        foreach (var reference in paper.ReferenceFrom)
        {
            var refPaper = _context.Papers.Include("Tags").FirstOrDefault(x => x.Id == reference.RefPaperId);
            if (refPaper != null)
            {
                references.Add(PaperInfo(refPaper));
            }
        }

        paperDetail["reference_from"] = referenceFrom;
        
        // 添加标签
        var tags = new JArray();
        foreach (var tagInfo in paper.Tags.Select(tag => _context.Tags.Find(tag.TagId)).Where(tagInfo => tagInfo != null))
        {
            tags.Add(JObject.FromObject(tagInfo));
        }

        paperDetail["tag"] = tags;
        paperDetail.Remove("tags");

        // TODO: 添加笔记
        var notes = new JArray();

        paperDetail["note"] = notes;
        paperDetail.Remove("notes");

        return paperDetail;
    }
}