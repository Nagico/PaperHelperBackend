using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;
using PaperHelper.Exceptions;
using PaperHelper.Services.Serializers;

namespace PaperHelper.Services;

/// <summary>
/// 论文管理服务
/// </summary>
public class PaperService
{
    private readonly IConfiguration _configuration;
    private readonly PaperHelperContext _context;
    private readonly PaperSerializer _paperSerializer;
    private readonly TagService _tagService;
    
    public PaperService(IConfiguration configuration, PaperHelperContext context)
    {
        _configuration = configuration;
        _context = context;
        _paperSerializer = new PaperSerializer(_context);
        _tagService = new TagService(_configuration, _context);
    }
    
    /// <summary>
    /// 获取详细论文对象
    /// </summary>
    /// <param name="id">论文ID</param>
    /// <returns>论文对象</returns>
    /// <exception cref="Exception"></exception>
    private Paper GetPaper(int id)
    {
        var paper = _context.Papers
            .Include("References").Include("ReferenceFrom")
            .Include("Tags").Include("Notes").Include("Attachment")
            .FirstOrDefault(x => x.Id == id);
        if (paper == null)
        {
            throw new AppError("A0510");
        }
        return paper;
    }

    private void CheckProjectMember(int projectId, int loginUserId)
    {
        var project = _context.Projects.Include("Members").First(p => p.Id == projectId);
        if (!project.Members.Any(m => m.UserId == loginUserId))
        {
            throw new AppError("A0312");
        }
    }
    
    /// <summary>
    /// 获取项目论文列表
    /// </summary>
    /// <param name="projectId">项目ID</param>
    /// <returns>论文列表</returns>
    public JArray GetProjectPapers(int projectId)
    {
        var paperList = _context.Papers.Where(p => p.ProjectId == projectId).ToList();
        var res = new JArray();
        foreach (var paper in paperList)
        {
            res.Add(_paperSerializer.PaperInfo(paper));
        }
        return res;
    }
    
    /// <summary>
    /// 获取论文详情
    /// </summary>
    /// <param name="paperId">论文ID</param>
    /// <returns>论文详情</returns>
    public JObject GetPaperDetail(int paperId)
    {
        var paper = GetPaper(paperId);
        return _paperSerializer.PaperDetail(paper);
    }
    
    /// <summary>
    /// 通过上传附件添加论文
    /// </summary>
    /// <param name="projectId">论文ID</param>
    /// <param name="fileName">文件名</param>
    /// <param name="extName">扩展名</param>
    /// <param name="formFile">文件</param>
    /// <returns></returns>
    public JObject CreatePaperWithAttachment(int projectId, string fileName, string extName, IFormFile formFile, int loginUserId)
    {
        CheckProjectMember(projectId, loginUserId);
        
        var attachmentService = new AttachmentService(_configuration, _context);
        var attachment = attachmentService.CreateAttachment(projectId, fileName, extName, formFile);
        
        var paper = new Paper
        {
            ProjectId = projectId,
            Title = fileName,
            CreateTime = DateTime.Now,
            UpdateTime = DateTime.Now,
            AttachmentId = attachment.Id
        };
        _context.Papers.Add(paper);
        _context.SaveChanges();
        
        paper = GetPaper(paper.Id);
        
        return _paperSerializer.PaperDetail(paper);
    }
    
    /// <summary>
    /// 更新属性
    /// </summary>
    private static void UpdateField(Paper paper, JObject paperJson, string objectFieldName, string jsonFieldName)
    {
        if (paperJson[jsonFieldName] == null) return;
        var field = paper.GetType().GetProperty(objectFieldName);
        if (field != null)
        {
            field.SetValue(paper, paperJson[jsonFieldName].ToString());
        }
    }
    
    /// <summary>
    /// 更新属性
    /// </summary>
    private static void UpdateFieldInt(Paper paper, JObject paperJson, string objectFieldName, string jsonFieldName)
    {
        if (paperJson[jsonFieldName] == null) return;
        var field = paper.GetType().GetProperty(objectFieldName);
        if (field != null)
        {
            field.SetValue(paper, int.Parse(paperJson[jsonFieldName]!.ToString()));
        }
    }
    
    /// <summary>
    /// 更新论文信息
    /// </summary>
    /// <param name="paperId">论文ID</param>
    /// <param name="paperJson">更新信息</param>
    /// <param name="loginUserId">当前登录用户ID</param>
    /// <returns>论文详情</returns>
    /// <exception cref="AppError">更新失败</exception>
    public JObject UpdatePaper(int paperId, JObject paperJson, int loginUserId)
    {
        var paper = GetPaper(paperId);
        CheckProjectMember(paper.ProjectId, loginUserId);
        
        // 更新属性
        UpdateField(paper, paperJson, "Title", "title");
        UpdateField(paper, paperJson, "Abstract", "abstract");
        UpdateField(paper, paperJson, "Keywords", "keyword");
        UpdateField(paper, paperJson, "Authors", "author");
        UpdateField(paper, paperJson, "AttachmentId", "attachmentId");
        UpdateField(paper, paperJson, "Publication", "publication");
        UpdateField(paper, paperJson, "Volume", "volume");
        UpdateField(paper, paperJson, "Pages", "pages");
        UpdateFieldInt(paper, paperJson, "Year", "year");
        UpdateFieldInt(paper, paperJson, "Month", "month");
        UpdateFieldInt(paper, paperJson, "Day", "day");
        UpdateField(paper, paperJson, "Doi", "doi");

        if (paperJson["url"] != null)
        {
            paper.Url = new Uri(paperJson["url"]!.ToString());
        }

        paper.UpdateTime = DateTime.Now;
        _context.SaveChanges();
        
        return _paperSerializer.PaperDetail(paper);
    }
    
    /// <summary>
    /// 新增论文标签
    /// </summary>
    /// <param name="paperId">论文ID</param>
    /// <param name="tagName">标签名</param>
    /// <param name="loginUserId">当前登录用户ID</param>
    /// <returns>Tag详情</returns>
    /// <exception cref="AppError">创建失败</exception>
    public JObject AddPaperTag(int paperId, string? tagName, int loginUserId)
    {
        var paper = _context.Papers.Include("Tags").FirstOrDefault(p => p.Id == paperId);
        
        if (paper == null)
        {
            throw new AppError("A0514");
        }
        
        CheckProjectMember(paper.ProjectId, loginUserId);
        
        // 创建tag
        var tag = _tagService.GetCreateTag(tagName);
        
        if (paper.Tags.Any(t => t.Id == tag.Id))
        {
            throw new AppError("A0430", "标签已存在");
        }
        
        paper.Tags.Add(new PaperTag
        {
            Paper = paper,
            Tag = tag
        });
        
        _context.SaveChanges();
        
        return JObject.FromObject(tag);
    }
    
    /// <summary>
    /// 删除论文标签
    /// </summary>
    /// <param name="paperId">论文ID</param>
    /// <param name="tagId">tagID</param>
    /// <param name="loginUserId">登录用户ID</param>
    /// <exception cref="AppError">删除失败</exception>
    public void DeletePaperTag(int paperId, int tagId, int loginUserId)
    {
        var paper = _context.Papers.Include("Tags").FirstOrDefault(p => p.Id == paperId);
        
        if (paper == null)
        {
            throw new AppError("A0514");
        }
        
        CheckProjectMember(paper.ProjectId, loginUserId);
        
        var paperTag = paper.Tags.FirstOrDefault(t => t.TagId == tagId);
        
        if (paperTag == null)
        {
            throw new AppError("A0514", "标签不存在");
        }

        paper.Tags.Remove(paperTag);
        
        _context.SaveChanges();
    }

}