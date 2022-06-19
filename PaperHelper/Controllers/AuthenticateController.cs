using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;
using PaperHelper.Exceptions;
using PaperHelper.Services;
using PaperHelper.ViewModels;

namespace PaperHelper.Controllers;

[ApiController]
[Route("api/tokens/")]
public class AuthenticateController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly PaperHelperContext _context;
    private readonly AuthenticateService _service;
    
    public AuthenticateController(IConfiguration configuration, PaperHelperContext paperHelperContext)
    {
        _configuration = configuration;
        _context = paperHelperContext;
        _service = new AuthenticateService(_configuration, _context);
    }
    
    [AllowAnonymous]
    [HttpPost("login", Name = "Login")]
    public ActionResult<AuthenticateViewModel> Login([FromBody] LoginViewModel loginViewModel)
    {
        var result = _service.Login(loginViewModel.Username, loginViewModel.Password);
        return result;
    }

    [AllowAnonymous]
    [HttpPost("register", Name = "Register")]
    public ActionResult<AuthenticateViewModel> Register([FromBody] RegisterViewModel registerViewModel)
    {
        var result = _service.Register(registerViewModel.Username, registerViewModel.Password, registerViewModel.Phone);
        return CreatedAtRoute("Login", result);
    }
}