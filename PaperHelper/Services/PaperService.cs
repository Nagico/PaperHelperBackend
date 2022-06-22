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
    private readonly UserSerializer _userSerializer;
    private readonly ProjectSerializer _projectSerializer;
    private readonly PaperSerializer _paperSerializer;
    
    public PaperService(IConfiguration configuration, PaperHelperContext context)
    {
        _configuration = configuration;
        _context = context;
        _userSerializer = new UserSerializer(_context);
        _projectSerializer = new ProjectSerializer(_context);
        _paperSerializer = new PaperSerializer(_context);
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
            .First(x => x.Id == id);
        if (paper == null)
        {
            throw new AppError("A0510");
        }
        return paper;
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
    public JObject CreatePaperWithAttachment(int projectId, string fileName, string extName, IFormFile formFile)
    {
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
    
}