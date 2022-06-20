using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PaperHelper.Entities;
using PaperHelper.Services;

namespace PaperHelper.Controllers;

[ApiController]
[Route("users")]
[Authorize]
public class UserController : BaseController
{
    private readonly UserService _service;
    
    public UserController(IConfiguration configuration, PaperHelperContext paperHelperContext)
    {
        _service = new UserService(configuration, paperHelperContext);
    }
    
    [HttpGet(Name = "GetUser")]
    public ActionResult GetUser()
    {
        var userDetail = _service.GetUserFullDetail(UserId);
        return Ok(userDetail);
    }
    
    [HttpGet("{id}", Name = "GetUserById")]
    public ActionResult GetUserById(int id)
    {
        var userDetail = UserId == id ? _service.GetUserFullDetail(id) : _service.GetUserPartDetail(id);
        return Ok(userDetail);
    }
    
    [HttpGet("{id}/projects", Name = "GetUserProjects")]
    public ActionResult GetUserProjects(int id)
    {
        var userProjects = _service.GetUserProjects(id);
        return Ok(userProjects);
    }
    
    
}