using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaperHelper.Entities;

namespace PaperHelper.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly PaperHelperContext _context;
    
    public TestController(PaperHelperContext context)
    {
        _context = context;
    }
    
    [HttpGet(Name = "Test")]
    public JsonResult Test()
    {
        return User.Identity?.IsAuthenticated switch
        {
            true => new JsonResult(new {username = User.Identity.Name, time = DateTime.Now, message = "login success"}),
            _ => new JsonResult(new {time = DateTime.Now, message = "test success"})
        };
    }
    
    [HttpGet("user", Name = "TestLoginUser")]
    [Authorize]
    public JsonResult TestLoginUser()
    {
        return new JsonResult(new {
            username = User.Identity?.Name,
            time = DateTime.Now,
            message = "login success"
        });
    }
}