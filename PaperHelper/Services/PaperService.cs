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
    /// 获取论文对象
    /// </summary>
    /// <param name="id">论文ID</param>
    /// <returns>论文对象</returns>
    /// <exception cref="Exception"></exception>
    private Paper GetPaper(int id)
    {
        var paper = _context.Papers.Find(id);
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
}