using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;
using PaperHelper.Exceptions;
using PaperHelper.Services.Serializers;

namespace PaperHelper.Services;

/// <summary>
/// Note管理服务
/// </summary>
public class NoteService
{
    private readonly IConfiguration _configuration;
    private readonly PaperHelperContext _context;
    private readonly NoteSerializer _noteSerializer;
    
    public NoteService(IConfiguration configuration, PaperHelperContext context)
    {
        _configuration = configuration;
        _context = context;
        _noteSerializer = new NoteSerializer(_context);
    }
    
    /// <summary>
    /// 检查权限
    /// </summary>
    /// <param name="paperId">论文ID</param>
    /// <param name="loginUserId">登录用户ID</param>
    /// <exception cref="AppError">无权限</exception>
    private void CheckMember(int paperId, int loginUserId)
    {
        var projectId = _context.Papers.Include("Project")
            .FirstOrDefault(x => x.Id == paperId)
            .ProjectId;
        var members = _context.Projects.Include("Members").First(x => x.Id == projectId).Members.ToList();
        if (members.All(x => x.UserId != loginUserId))
        {
            throw new AppError("A0514");
        }
    }
    
    /// <summary>
    /// 获取笔记列表
    /// </summary>
    /// <param name="id">论文ID</param>
    /// <param name="loginUserId">当前登录用户ID</param>
    /// <returns>笔记列表JSON</returns>
    public JArray GetNotes(int id, int loginUserId)
    {
        CheckMember(id, loginUserId);
        var notes = _context.Notes.Where(n => n.PaperId == id && n.Type == 0).ToList();
        return JArray.FromObject(notes);
    }
    
    /// <summary>
    /// 创建或获取思维导图
    /// </summary>
    /// <param name="id">论文ID</param>
    /// <param name="loginUserId">当前登录用户</param>
    /// <returns>思维导图信息JSON</returns>
    public JObject GetCreateMindMap(int id, int loginUserId)
    {
        CheckMember(id, loginUserId);
        
        var note = _context.Notes.FirstOrDefault(n => n.PaperId == id && n.Type == 1);
        
        // 已存在的MindMap
        if (note != null) return _noteSerializer.MindMap(note);
        
        // 新建MindMap
        note = new Note
        {
            PaperId = id,
            Type = 1,
            Content = "{}",
            CreateTime = DateTime.Now,
            UpdateTime = DateTime.Now,
        };
        _context.Notes.Add(note);
        _context.SaveChanges();
        return _noteSerializer.MindMap(note);
    }
    
    /// <summary>
    /// 创建或获取批注
    /// </summary>
    /// <param name="id">论文ID</param>
    /// <param name="loginUserId">当前登录用户</param>
    /// <returns>批注信息JSON</returns>
    public JObject GetCreateAnnotation(int id, int loginUserId)
    {
        CheckMember(id, loginUserId);
        
        var note = _context.Notes.FirstOrDefault(n => n.PaperId == id && n.Type == 2);

        // 已存在的Annotation
        if (note != null) return JObject.FromObject(note);

        // 新建Annotation
        note = new Note
        {
            PaperId = id,
            Type = 2,
            Content = "",
            CreateTime = DateTime.Now,
            UpdateTime = DateTime.Now,
        };
        _context.Notes.Add(note);
        _context.SaveChanges();
        return JObject.FromObject(note);
    }
    
    /// <summary>
    /// 更新思维导图
    /// </summary>
    /// <param name="id">论文ID</param>
    /// <param name="content">JSON内容</param>
    /// <param name="loginUserId">登录用户ID</param>
    /// <returns>思维导图详情</returns>
    public JObject UpdateMindMap(int id, JObject content, int loginUserId)
    {
        CheckMember(id, loginUserId);
        
        var note = _context.Notes.FirstOrDefault(n => n.PaperId == id && n.Type == 1);
        if (note == null)
        {
            note = new Note
            {
                PaperId = id,
                Title = "MindMap",
                Type = 1,
                Content = content.ToString(),
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
            };
            _context.Notes.Add(note);
        }
        else
        {
            note.Content = content.ToString();
            note.UpdateTime = DateTime.Now;
        }
        _context.SaveChanges();
        return _noteSerializer.MindMap(note);
    }
    
    /// <summary>
    /// 更新批注
    /// </summary>
    /// <param name="id">论文ID</param>
    /// <param name="content">批注内容</param>
    /// <param name="loginUserId">登录用户ID</param>
    /// <returns>批注详情</returns>
    public JObject UpdateAnnotation(int id, string content, int loginUserId)
    {
        CheckMember(id, loginUserId);
        
        var note = _context.Notes.FirstOrDefault(n => n.PaperId == id && n.Type == 2);
        if (note == null)
        {
            note = new Note
            {
                PaperId = id,
                Title = "Annotation",
                Type = 2,
                Content = content,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
            };
            _context.Notes.Add(note);
        }
        else
        {
            note.Content = content;
            note.UpdateTime = DateTime.Now;
        }
        _context.SaveChanges();
        return JObject.FromObject(note);
    }
}