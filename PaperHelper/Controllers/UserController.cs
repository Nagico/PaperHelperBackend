using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;
using PaperHelper.Services;
using PaperHelper.Services.Serializers;

namespace PaperHelper.Controllers;

[ApiController]
[Route("users")]
[Authorize]
public class UserController : BaseController
{
    private readonly UserService _userService;
    private readonly ProjectService _projectService;
    
    public UserController(IConfiguration configuration, PaperHelperContext paperHelperContext)
    {
        _userService = new UserService(configuration, paperHelperContext);
        _projectService = new ProjectService(configuration, paperHelperContext);
    }
    
    [HttpGet(Name = "GetUser")]
    public ActionResult GetUser()
    {
        var userDetail = _userService.GetUserFullDetail(UserId);
        return Ok(userDetail);
    }
    
    [HttpGet("{id}", Name = "GetUserById")]
    public ActionResult GetUserById(int id)
    {
        var userDetail = UserId == id ? _userService.GetUserFullDetail(id) : _userService.GetUserPartDetail(id);
        return Ok(userDetail);
    }
    
    [HttpGet("{id}/projects", Name = "GetUserProjects")]
    public ActionResult GetUserProjects(int id)
    {
        var userProjects = _projectService.GetUserProjects(id);
        return Ok(userProjects);
    }
    
    
}