using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaperHelper.Entities;

namespace PaperHelper.Controllers;

[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
    private readonly PaperHelperContext _context;
    
    public TestController(PaperHelperContext context)
    {
        _context = context;
    }
    
    [HttpGet(Name = "Test")]
    public ActionResult Test()
    {
        return User.Identity?.IsAuthenticated switch
        {
            true => Ok(new {Username = User.Identity.Name, Time = DateTime.Now, Message = "login success"}),
            _ => Ok(new {Time = DateTime.Now, Message = "test success"})
        };
    }
    
    [HttpGet("user", Name = "TestLoginUser")]
    [Authorize]
    public ActionResult TestLoginUser()
    {
        return Ok(new {
            Username = User.Identity?.Name,
            Time = DateTime.Now,
            Message = "login success"
        });
    }
}