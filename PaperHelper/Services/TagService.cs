using PaperHelper.Entities;
using PaperHelper.Entities.Entities;
using PaperHelper.Exceptions;

namespace PaperHelper.Services;

/// <summary>
/// 标签管理服务
/// </summary>
public class TagService
{
    private readonly IConfiguration _configuration;
    private readonly PaperHelperContext _context;
    
    public TagService(IConfiguration configuration, PaperHelperContext context)
    {
        _configuration = configuration;
        _context = context;
    }
    
    /// <summary>
    /// 获取标签对象
    /// </summary>
    /// <param name="tagName">标签名</param>
    /// <returns>标签对象</returns>
    /// <exception cref="AppError">标签不存在</exception>
    public Tag GetTag(string? tagName)
    {
        if (string.IsNullOrEmpty(tagName))
        {
            throw new AppError("A0400", "标签名称不能为空");
        }
        return _context.Tags.FirstOrDefault(t => t.Name == tagName) ?? throw new AppError("A0510", "标签不存在");
    }
    
    /// <summary>
    /// 获取标签对象
    /// </summary>
    /// <param name="tagId">标签ID</param>
    /// <returns>标签对象</returns>
    /// <exception cref="AppError">标签不存在</exception>
    public Tag GetTag(int tagId)
    {
        return _context.Tags.FirstOrDefault(t => t.Id == tagId) ?? throw new AppError("A0510", "标签不存在");
    }
    
    /// <summary>
    /// 创建或获取标签对象
    /// </summary>
    /// <param name="tagName">标签名</param>
    /// <returns>标签对象</returns>
    /// <exception cref="AppError">创建失败</exception>
    public Tag GetCreateTag(string? tagName)
    {
        if (string.IsNullOrEmpty(tagName))
        {
            throw new AppError("A0400", "标签名称不能为空");
        }
        
        // 尝试获取标签
        var tag = _context.Tags.FirstOrDefault(t => t.Name == tagName);
        
        // 标签存在
        if (tag != null) return tag;
        
        // 标签不存在，创建标签
        tag = new Tag
        {
            Name = tagName
        };
        _context.Tags.Add(tag);
        _context.SaveChanges();
        return tag;
    }
}