using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaperHelper.Entities;

namespace PaperHelper.Controllers;

[ApiController]
[Route("test")]
public class TestController : BaseController
{
    /// <summary>
    /// 匿名测试接口
    /// </summary>
    /// <returns>测试结果</returns>
    [HttpGet(Name = "Test")]
    public ActionResult Test()
    {
        return User.Identity?.IsAuthenticated switch
        {
            true => Ok(new {Username = User.Identity.Name, Time = DateTime.Now, Message = "login success"}),
            _ => Ok(new {Time = DateTime.Now, Message = "test success"})
        };
    }
    
    /// <summary>
    /// 认证用户测试接口
    /// </summary>
    /// <returns>用户名及结果</returns>
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