using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaperHelper.Entities;
using PaperHelper.Services;
using PaperHelper.ViewModels;

namespace PaperHelper.Controllers;

[ApiController]
[Route("tokens")]
public class AuthenticateController : BaseController
{
    private readonly AuthenticateService _service;
    
    public AuthenticateController(IConfiguration configuration, PaperHelperContext paperHelperContext)
    {
        _service = new AuthenticateService(configuration, paperHelperContext);
    }
    
    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="loginViewModel">登录信息</param>
    /// <returns>token及用户信息</returns>
    [AllowAnonymous]
    [HttpPost("login", Name = "Login")]
    public ActionResult<AuthenticateViewModel> Login([FromBody] LoginViewModel loginViewModel)
    {
        var result = _service.Login(loginViewModel.Username, loginViewModel.Password);
        return result;
    }
    
    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="registerViewModel">注册信息</param>
    /// <returns>token及用户信息</returns>
    [AllowAnonymous]
    [HttpPost("register", Name = "Register")]
    public ActionResult<AuthenticateViewModel> Register([FromBody] RegisterViewModel registerViewModel)
    {
        var result = _service.Register(registerViewModel.Username, registerViewModel.Password, registerViewModel.Phone);
        return CreatedAtRoute("Login", result);
    }
    
    /// <summary>
    /// 获取用户名数量
    /// </summary>
    /// <param name="username">用户名</param>
    /// <returns>数据库中的数量</returns>
    [AllowAnonymous]
    [HttpGet("count/{username}", Name = "Count")]
    public ActionResult Count(string username)
    {
        var result = _service.UsernameCount(username);
        return Ok(new { count = result });
    }
}