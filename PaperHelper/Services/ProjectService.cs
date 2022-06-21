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
        var project =  _context.Projects.Find(id);
        if (project == null)
        {
            throw new AppError("A0514");
        }
        return project;
    }

    public JArray GetUserProjects(int userId)
    {
        var projects = _context.UserProjects.Where(x => x.UserId == userId);
        var res = new JArray();
        foreach (var project in projects)
        {
            res.Add(_projectSerializer.ProjectInfo(project.Project));
        }
        return res;
    }

    // public JObject GetProjectDetail(int id)
    // {
    //         var project = GetProject(id);
    //     return _projectSerializer.ProjectDetail(project);
    // }
}