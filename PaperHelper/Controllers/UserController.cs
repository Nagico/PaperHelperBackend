using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;
using PaperHelper.Exceptions;
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
    
    [HttpGet("{id}/projects", Name = "UserGetUserProjects")]
    public ActionResult UserGetUserProjects(int id)
    {
        if (UserId != id)
        {
            throw new AppError("A0312");
        }
        var userProjects = _projectService.GetUserProjects(id);
        return Ok(userProjects);
    }

    [HttpPost("{id}/projects", Name = "UserJoinProject")]
    public ActionResult UserJoinProject(int id, [FromBody] JObject body)
    {
        if (UserId != id)
        {
            throw new AppError("A0312");
        }
        var code = body.GetValue("invitation_code");
        if (code == null)
        {
            throw new AppError("A0312");
        }
        var res = _userService.JoinProject(id, code.ToString());

        return Ok(res);
    }
}