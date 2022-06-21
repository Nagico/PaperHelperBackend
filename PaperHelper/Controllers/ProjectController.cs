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
        var updatedProject = _projectService.UpdateProjectInfo(id, project.Name, project.Description, UserId);
        return Ok(updatedProject);
    }
    
    [HttpDelete("{id}", Name = "DeleteProject")]
    public ActionResult DeleteProject(int id)
    {
        _projectService.DeleteProject(id, UserId);
        return NoContent();
    }
    
    [HttpPost("{id}/members/{userId}", Name = "AddMemberToProject")]
    public ActionResult AddMemberToProject(int id, int userId)
    {
        var newMember = _projectService.AddMember(id, userId, UserId);
        return Created($"/projects/{id}", newMember);
    }
    
    [HttpDelete("{id}/members/{userId}", Name = "RemoveMemberFromProject")]
    public ActionResult RemoveMemberFromProject(int id, int userId)
    {
        _projectService.RemoveMember(id, userId, UserId);
        return NoContent();
    }
    
    [HttpPut("{id}/owners/{userId}", Name = "TransProjectOwner")]
    public ActionResult TransProjectOwner(int id, int userId)
    {
        var res = _projectService.TransOwner(id, userId, UserId);
        return Ok(res);
    }
}