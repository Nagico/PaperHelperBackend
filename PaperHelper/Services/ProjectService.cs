using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;
using PaperHelper.Exceptions;
using PaperHelper.Services.Serializers;

namespace PaperHelper.Services;

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
    
    private Project GetProject(int id)
    {
        var project =  _context.Projects.Include("Members").FirstOrDefault(p => p.Id == id);
        if (project == null)
        {
            throw new AppError("A0514");
        }
        return project;
    }

    public JArray GetUserProjects(int userId)
    {
        var projectIds = _context.UserProjects.Where(x => x.UserId == userId).ToList();
        var res = new JArray();
        foreach (var project in projectIds.Select(projectId => GetProject(projectId.ProjectId)))
        {
            res.Add(_projectSerializer.ProjectInfo(project));
        }
        return res;
    }

    public JObject GetProjectDetail(int id)
    {
        var project = GetProject(id);
        return _projectSerializer.ProjectDetail(project);
    }

    public JObject CreateProject(string? name, string? description, int userId)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description))
        {
            throw new AppError("A0420");
        }
        var user = _context.Users.Find(userId);
        if (user == null)
        {
            throw new AppError("A0300");
        }
        
        var project = new Project
        {
            Name = name,
            Description = description,
            Members = new List<UserProject>(),
            UpdateTime = DateTime.Now,
            CreateTime = DateTime.Now
        };
        project.Members.Add(new UserProject
        {
            User = user,
            Project = project,
            IsOwner = true,
            AccessTime = DateTime.Now,
            CreateTime = DateTime.Now
        });
        _context.Projects.Add(project);
        _context.SaveChanges();
        return _projectSerializer.ProjectDetail(project);
    }

    public JObject UpdateProjectInfo(int id, string? projectName, string? projectDescription)
    {
        var project = GetProject(id);
        if (projectName != null)
        {
            project.Name = projectName;
        }
        if (projectDescription != null)
        {
            project.Description = projectDescription;
        }
        project.UpdateTime = DateTime.Now;
        _context.SaveChanges();
        return _projectSerializer.ProjectDetail(project);
    }

    public JObject AddMember(int id, int userId, int currentUserId)
    {
        var project = GetProject(id);
        if (!project.Members.Any(x => x.UserId == userId))
        {
            throw new AppError("A0312");
        }
        var user = _context.Users.Find(userId);
        if (user == null)
        {
            throw new AppError("A0430", "用户不存在");
        }
        if (project.Members.Any(x => x.UserId == userId))
        {
            throw new AppError("A0430", "用户重复加入");
        }
        project.Members.Add(new UserProject
        {
            User = user,
            Project = project,
            IsOwner = false,
            AccessTime = DateTime.Now,
            CreateTime = DateTime.Now
        });
        project.UpdateTime = DateTime.Now;
        _context.SaveChanges();
        return _projectSerializer.ProjectDetail(project);
    }
    
    public JObject RemoveMember(int id, int userId, int currentUserId)
    {
        var project = GetProject(id);
        if (!project.Members.Any(x => x.UserId == userId))
        {
            throw new AppError("A0312");
        }
        var user = _context.Users.Find(userId);
        if (user == null)
        {
            throw new AppError("A0430", "用户不存在");
        }
        if (project.Members.All(x => x.UserId != userId))
        {
            throw new AppError("A0430", "用户未加入");
        }
        var member = project.Members.First(x => x.UserId == userId);
        project.Members.Remove(member);
        project.UpdateTime = DateTime.Now;
        _context.SaveChanges();
        return _projectSerializer.ProjectDetail(project);
    }
    
    public void DeleteProject(int id, int userId)
    {
        var project = GetProject(id);
        if (!project.Members.Any(x => x.UserId == userId && x.IsOwner))
        {
            throw new AppError("A0430", "仅所有者能删除项目");
        }
        _context.UserProjects.RemoveRange(_context.UserProjects.Where(x => x.ProjectId == id));
        _context.Projects.Remove(project);
        _context.SaveChanges();
    }
}