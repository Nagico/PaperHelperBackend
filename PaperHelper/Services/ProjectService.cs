using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;
using PaperHelper.Exceptions;
using PaperHelper.Services.Serializers;
using PaperHelper.Utils;

namespace PaperHelper.Services;

/// <summary>
/// 项目管理服务
/// </summary>
public class ProjectService
{
    private readonly IConfiguration _configuration;
    private readonly PaperHelperContext _context;
    private readonly UserSerializer _userSerializer;
    private readonly ProjectSerializer _projectSerializer;
    
    public ProjectService(IConfiguration configuration, PaperHelperContext context)
    {
        _configuration = configuration;
        _context = context;
        _userSerializer = new UserSerializer(_context);
        _projectSerializer = new ProjectSerializer(_context);
    }
    
    /// <summary>
    /// 是否为项目成员
    /// </summary>
    /// <param name="project">项目</param>
    /// <param name="userId">用户ID</param>
    /// <returns>查询结果</returns>
    private bool IsMember(Project project, int userId)
    {
        return project.Members.Any(member => member.UserId == userId);
    }
    
    /// <summary>
    /// 是否为项目拥有者
    /// </summary>
    /// <param name="project">项目</param>
    /// <param name="userId">用户ID</param>
    /// <returns>查询结果</returns>
    private bool IsOwner(Project project, int userId)
    {
        return project.Members.Any(member => member.UserId == userId && member.IsOwner);
    }
    
    /// <summary>
    /// 检查成员权限
    /// </summary>
    /// <param name="project">项目</param>
    /// <param name="userId">用户ID</param>
    /// <exception cref="AppError">无权限</exception>
    private void CheckMember(Project project, int userId)
    {
        if (!IsMember(project, userId))
        {
            throw new AppError("A0312", "非项目成员");
        }
    }
    
    /// <summary>
    /// 检查所有者权限
    /// </summary>
    /// <param name="project">项目</param>
    /// <param name="userId">用户ID</param>
    /// <exception cref="AppError">无权限</exception>
    private void CheckOwner(Project project, int userId)
    {
        if (!IsOwner(project, userId))
        {
            throw new AppError("A0313", "非项目所有者");
        }
    }
    
    /// <summary>
    /// 获取项目及子对象
    /// </summary>
    /// <param name="id">项目ID</param>
    /// <returns>项目对象</returns>
    /// <exception cref="AppError">ID不存在</exception>
    private Project GetProject(int id)
    {
        var project =  _context.Projects.Include("Members").Include("Papers").FirstOrDefault(p => p.Id == id);
        if (project == null)
        {
            throw new AppError("A0514");
        }
        return project;
    }
    
    /// <summary>
    /// 获取用户项目列表
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>项目信息列表</returns>
    public JArray GetUserProjects(int userId)
    {
        var projectIds = _context.UserProjects.Where(x => x.UserId == userId).ToList();
        var res = new JArray();
        
        // 获取项目列表
        foreach (var project in projectIds.Select(projectId => GetProject(projectId.ProjectId)))
        {
            res.Add(_projectSerializer.ProjectInfo(project));
        }
        return res;
    }
    
    /// <summary>
    /// 获取项目详情
    /// </summary>
    /// <param name="id">项目ID</param>
    /// <returns>项目信息</returns>
    public JObject GetProjectDetail(int id)
    {
        var project = GetProject(id);
        return _projectSerializer.ProjectDetail(project);
    }
    
    /// <summary>
    /// 创建项目
    /// </summary>
    /// <param name="name">项目名</param>
    /// <param name="description">项目描述</param>
    /// <param name="userId">创建者ID</param>
    /// <returns>项目详情</returns>
    /// <exception cref="AppError">创建出错</exception>
    public JObject CreateProject(string? name, string? description, int userId)
    {
        // 参数校验
        if (string.IsNullOrEmpty(name))
        {
            throw new AppError("A0420");
        }
        
        // 检查用户是否存在
        var user = _context.Users.Find(userId);
        if (user == null)
        {
            throw new AppError("A0300");
        }
        
        // 新项目
        var project = new Project
        {
            Name = name,
            Description = description,
            Members = new List<UserProject>(),
            UpdateTime = DateTime.Now,
            CreateTime = DateTime.Now,
            InvitationCode = CodeUtil.GenInvitationCode(8)
        };
        
        // 添加所有者
        project.Members.Add(new UserProject
        {
            User = user,
            Project = project,
            IsOwner = true,
            AccessTime = DateTime.Now,
            CreateTime = DateTime.Now
        });
        
        // 添加项目
        _context.Projects.Add(project);
        _context.SaveChanges();
        
        // 返回详情
        return _projectSerializer.ProjectDetail(project);
    }
    
    /// <summary>
    /// 修改项目信息
    /// </summary>
    /// <param name="id">项目ID</param>
    /// <param name="projectName">项目名</param>
    /// <param name="projectDescription">项目描述</param>
    /// <param name="loginUserId">当前登录用户ID</param>
    /// <returns>修改后项目详情</returns>
    public JObject UpdateProjectInfo(int id, string? projectName, string? projectDescription, int loginUserId)
    {
        // 获取项目
        var project = GetProject(id);
        
        // 检查权限
        CheckMember(project, loginUserId);
        
        // 修改项目信息
        if (projectName != null)
        {
            project.Name = projectName;
        }
        if (projectDescription != null)
        {
            project.Description = projectDescription;
        }
        
        // 修改UpdateTime
        project.UpdateTime = DateTime.Now;
        
        // 保存
        _context.SaveChanges();
        return _projectSerializer.ProjectDetail(project);
    }
    
    /// <summary>
    /// 添加用户到项目
    /// </summary>
    /// <param name="id">项目ID</param>
    /// <param name="userId">待添加用户ID</param>
    /// <param name="loginUserId">当前登录用户ID</param>
    /// <returns>项目详情</returns>
    /// <exception cref="AppError">添加失败</exception>
    public JObject AddMember(int id, int userId, int loginUserId)
    {
        // 获取项目
        var project = GetProject(id);
        
        // 检查权限
        CheckMember(project, loginUserId);
        
        // 检查用户是否存在
        var user = _context.Users.Find(userId);
        if (user == null)
        {
            throw new AppError("A0430", "用户不存在");
        }
        
        // 检查用户是否已经在项目中
        if (project.Members.Any(x => x.UserId == userId))
        {
            throw new AppError("A0430", "用户重复加入");
        }
        
        // 添加用户
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
    
    /// <summary>
    /// 删除成员
    /// </summary>
    /// <param name="id">项目ID</param>
    /// <param name="userId">待删除用户ID</param>
    /// <param name="loginUserId">当前登录用户ID</param>
    /// <returns>项目详情</returns>
    /// <exception cref="AppError">删除失败</exception>
    public JObject RemoveMember(int id, int userId, int loginUserId)
    {
        // 获取项目
        var project = GetProject(id);
        
        // 检查权限
        CheckMember(project, loginUserId);
        
        // 所有者不能被移除
        if (IsOwner(project, loginUserId))
        {
            throw new AppError("A0430", "项目所有者不能移除");
        }
        
        // 检查用户是否存在
        var user = _context.Users.Find(userId);
        if (user == null)
        {
            throw new AppError("A0430", "用户不存在");
        }
        
        // 检查用户是否在项目中
        if (project.Members.All(x => x.UserId != userId))
        {
            throw new AppError("A0430", "用户未加入");
        }
        
        // 删除用户
        var member = project.Members.First(x => x.UserId == userId);
        project.Members.Remove(member);
        project.UpdateTime = DateTime.Now;
        
        // 保存
        _context.SaveChanges();
        return _projectSerializer.ProjectDetail(project);
    }
    
    /// <summary>
    /// 删除项目
    /// </summary>
    /// <param name="id">项目ID</param>
    /// <param name="loginUserId">当前登录用户ID</param>
    public void DeleteProject(int id, int loginUserId)
    {
        // 获取项目
        var project = GetProject(id);
        
        // 检查权限
        CheckOwner(project, loginUserId);
        
        // 删除项目成员
        _context.UserProjects.RemoveRange(_context.UserProjects.Where(x => x.ProjectId == id));
        
        // 删除项目
        _context.Projects.Remove(project);
        
        // 保存
        _context.SaveChanges();
    }
    
    /// <summary>
    /// 转移项目所有者
    /// </summary>
    /// <param name="id">项目ID</param>
    /// <param name="userId">带转移用户ID</param>
    /// <param name="loginUserId">当前登录用户ID</param>
    /// <returns>项目详情</returns>
    public JObject TransOwner(int id, int userId, int loginUserId)
    {
        // 获取项目
        var project = GetProject(id);
        
        // 检查权限
        CheckOwner(project, loginUserId);
        
        if (IsOwner(project, userId))
        {
            throw new AppError("A0430", "用户已是项目所有者");
        }
        
        // 转移项目
        project.Members.First(x => x.UserId == userId).IsOwner = true;
        project.Members.First(x => x.UserId == loginUserId).IsOwner = false;
        project.UpdateTime = DateTime.Now;
        
        // 保存
        _context.SaveChanges();
        return _projectSerializer.ProjectDetail(project);
    }
}