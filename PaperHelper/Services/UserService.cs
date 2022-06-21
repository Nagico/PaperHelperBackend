using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;
using PaperHelper.Exceptions;
using PaperHelper.Services.Serializers;

namespace PaperHelper.Services;

/// <summary>
/// 用户服务
/// </summary>
public class UserService
{
    private readonly IConfiguration _configuration;
    private readonly PaperHelperContext _context;
    private readonly UserSerializer _userSerializer;
    private readonly ProjectSerializer _projectSerializer;
    
    public UserService(IConfiguration configuration, PaperHelperContext paperHelperContext)
    {
        _configuration = configuration;
        _context = paperHelperContext;
        _userSerializer = new UserSerializer(_context);
        _projectSerializer = new ProjectSerializer(_context);
    }
    
    /// <summary>
    /// 获取用户对象
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>用户对象</returns>
    /// <exception cref="AppError">用户不存在</exception>
    private User GetUser(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            throw new AppError("A0514");
        }
        return user;
    }
    
    /// <summary>
    /// 获取用户简要信息
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>JSON对象</returns>
    public JObject GetUserPartDetail(int id)
    {
        var user = GetUser(id);
        
        return _userSerializer.UserInfo(user);
    }
    
    /// <summary>
    /// 获取用户全部详细信息
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>JSON对象</returns>
    public JObject GetUserFullDetail(int id)
    {
        var user = GetUser(id);
        
        return _userSerializer.UserDetail(user);
    }
    
    /// <summary>
    /// 加入项目
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <param name="invitationCode">邀请码</param>
    /// <returns>项目详情</returns>
    /// <exception cref="AppError">加入失败</exception>
    public JObject JoinProject(int id, string invitationCode)
    {
        // 查找邀请码对应的项目
        var project = _context.Projects.Include("Members").First(x => x.InvitationCode == invitationCode);
        if (project == null)
        {
            throw new AppError("A0430", "邀请码错误");
        }
        
        // 查找用户
        var user = _context.Users.Find(id);
        if (user == null)
        {
            throw new AppError("A0430", "用户不存在");
        }
        
        // 判断用户是否已经加入项目
        if (project.Members.Any(x => x.UserId == id))
        {
            throw new AppError("A0430", "用户重复加入");
        }
        
        // 添加用户到项目
        project.Members.Add(new UserProject
        {
            User = user,
            Project = project,
            IsOwner = false,
            AccessTime = DateTime.Now,
            CreateTime = DateTime.Now
        });
        project.UpdateTime = DateTime.Now;
        
        // 保存
        _context.SaveChanges();
        return _projectSerializer.ProjectDetail(project);
    }
}