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
    
    /// <summary>
    /// 获取用户所有项目
    /// </summary>
    /// <returns>项目简要信息列表</returns>
    [HttpGet(Name = "ProjectGetUserProjects")]
    public ActionResult ProjectGetUserProjects()
    {
        var projects = _projectService.GetUserProjects(UserId);
        return Ok(projects);
    }
    
    /// <summary>
    /// 获取项目详情
    /// </summary>
    /// <param name="id">项目ID</param>
    /// <returns>项目内容</returns>
    [HttpGet("{id}", Name = "GetProject")]
    public ActionResult GetProject(int id)
    {
        var project = _projectService.GetProjectDetail(id);
        return Ok(project);
    }
    
    /// <summary>
    /// 新增项目
    /// </summary>
    /// <param name="project">项目信息</param>
    /// <returns></returns>
    [HttpPost(Name = "CreateProject")]
    public ActionResult CreateProject([FromBody] Project project)
    {
        var newProject = _projectService.CreateProject(project.Name, project.Description, UserId);
        return Created($"/projects/{newProject["id"]}", newProject);
    }
    
    /// <summary>
    /// 修改项目
    /// </summary>
    /// <param name="id">项目ID</param>
    /// <param name="project">修改后内容</param>
    /// <returns></returns>
    [HttpPut("{id}", Name = "UpdateProjectInfo")]
    public ActionResult UpdateProjectInfo(int id, [FromBody] Project project)
    {
        var updatedProject = _projectService.UpdateProjectInfo(id, project.Name, project.Description, UserId);
        return Ok(updatedProject);
    }
    
    /// <summary>
    /// 删除项目
    /// </summary>
    /// <param name="id">项目ID</param>
    /// <returns></returns>
    [HttpDelete("{id}", Name = "DeleteProject")]
    public ActionResult DeleteProject(int id)
    {
        _projectService.DeleteProject(id, UserId);
        return NoContent();
    }
    
    /// <summary>
    /// 添加项目成员（弃用）
    /// </summary>
    /// <param name="id">项目ID</param>
    /// <param name="userId">成员ID</param>
    /// <returns>项目详情</returns>
    [Obsolete("接口弃用，添加用户请使用邀请码", true)]
    [HttpPost("{id}/members/{userId}", Name = "AddMemberToProject")]
    public ActionResult AddMemberToProject(int id, int userId)
    {
        var newMember = _projectService.AddMember(id, userId, UserId);
        return Created($"/projects/{id}", newMember);
    }
    
    /// <summary>
    /// 删除项目成员
    /// </summary>
    /// <param name="id">项目ID</param>
    /// <param name="userId">成员ID</param>
    /// <returns>空</returns>
    [HttpDelete("{id}/members/{userId}", Name = "RemoveMemberFromProject")]
    public ActionResult RemoveMemberFromProject(int id, int userId)
    {
        _projectService.RemoveMember(id, userId, UserId);
        return NoContent();
    }
    
    /// <summary>
    /// 迁移项目所有者
    /// </summary>
    /// <param name="id">项目ID</param>
    /// <param name="userId">新所有者ID</param>
    /// <returns>项目详情</returns>
    [HttpPut("{id}/owners/{userId}", Name = "TransProjectOwner")]
    public ActionResult TransProjectOwner(int id, int userId)
    {
        var res = _projectService.TransOwner(id, userId, UserId);
        return Ok(res);
    }
}