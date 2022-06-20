using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaperHelper.Entities;

namespace PaperHelper.Controllers;

[ApiController]
[Route("test")]
public class TestController : BaseController
{
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