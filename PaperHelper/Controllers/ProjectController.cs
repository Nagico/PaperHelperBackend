using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;
using PaperHelper.Services;

namespace PaperHelper.Controllers;

[ApiController]
[Route("projects")]
[Authorize]
public class ProjectController : BaseController
{
    private readonly ProjectService _projectService;
    
    public ProjectController(IConfiguration configuration, PaperHelperContext paperHelperContext)
    {
        _projectService = new ProjectService(configuration, paperHelperContext);
    }
    
    [HttpGet(Name = "ProjectGetUserProjects")]
    public ActionResult ProjectGetUserProjects()
    {
        var projects = _projectService.GetUserProjects(UserId);
        return Ok(projects);
    }
    
    [HttpGet("{id}", Name = "GetProject")]
    public ActionResult GetProject(int id)
    {
        var project = _projectService.GetProjectDetail(id);
        return Ok(project);
    }
    
    [HttpPost(Name = "CreateProject")]
    public ActionResult CreateProject([FromBody] Project project)
    {
        var newProject = _projectService.CreateProject(project.Name, project.Description, UserId);
        return Created($"/projects/{newProject["id"]}", newProject);
    }
    
    [HttpPut("{id}", Name = "UpdateProjectInfo")]
    public ActionResult UpdateProjectInfo(int id, [FromBody] Project project)
    {
        var updatedProject = _projectService.UpdateProjectInfo(id, project.Name, project.Description);
        return Ok(updatedProject);
    }
}