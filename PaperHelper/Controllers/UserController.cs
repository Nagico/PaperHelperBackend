using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaperHelper.Entities;
using PaperHelper.Services;

namespace PaperHelper.Controllers;

[ApiController]
[Route("users")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserService _service;
    
    public UserController(IConfiguration configuration, PaperHelperContext paperHelperContext)
    {
        _service = new UserService(configuration, paperHelperContext);
    }
    
    [HttpGet(Name = "GetUser")]
    public ActionResult GetUser()
    {
        var id = 1;
        var user = _service.GetUserDetail(id);
        return Ok(user);
    }
}