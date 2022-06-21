using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;
using PaperHelper.Exceptions;
using PaperHelper.Services.Serializers;

namespace PaperHelper.Services;

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

    private User GetUser(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            throw new AppError("A0514");
        }
        return user;
    }
    
    public JObject GetUserPartDetail(int id)
    {
        var user = GetUser(id);
        
        return _userSerializer.UserInfo(user);
    }
    
    public JObject GetUserFullDetail(int id)
    {
        var user = GetUser(id);
        
        return _userSerializer.UserDetail(user);
    }
    
    public JObject JoinProject(int id, string invitationCode)
    {
        var project = _context.Projects.Include("Members").First(x => x.InvitationCode == invitationCode);
        var user = _context.Users.Find(id);
        if (project == null)
        {
            throw new AppError("A0430", "邀请码错误");
        }
        if (user == null)
        {
            throw new AppError("A0430", "用户不存在");
        }
        if (project.Members.Any(x => x.UserId == id))
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
}