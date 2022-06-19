using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;
using PaperHelper.ViewModels;

namespace PaperHelper.Controllers;

[ApiController]
[Route("api/tokens/")]
public class AuthenticateController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly PaperHelperContext _context;
    
    public AuthenticateController(IConfiguration configuration, PaperHelperContext paperHelperContext)
    {
        _configuration = configuration;
        _context = paperHelperContext;
    }
    
    [AllowAnonymous]
    [HttpPost("login", Name = "Login")]
    public JsonResult Login([FromBody] LoginViewModel loginViewModel)
    {
        //TODO: 统一异常 错误处理
        //User Authentication
        if (string.IsNullOrWhiteSpace(loginViewModel.Username) || string.IsNullOrWhiteSpace(loginViewModel.Password))
        {
            return new JsonResult(new {
                message = "Username or Password is empty"
            });
        }
        
        var user = _context.Users.FirstOrDefault(u => u.Username == loginViewModel.Username);
        if (user == null || !user.Password.Equals(loginViewModel.Password)) // TODO: 密码加密
        {
            return new JsonResult(new {
                message = "Username or Password is incorrect"
            });
        }

        //Generate Token
        var token = GenerateJwt(loginViewModel.Username);
        
        return new JsonResult(new {
            token = token,
            username = loginViewModel.Username
        });
    }

    [AllowAnonymous]
    [HttpPost("register", Name = "Register")]
    public JsonResult Register([FromBody] RegisterViewModel registerViewModel)
    {
        _context.Users.Add(new User {
            Username = registerViewModel.Username,
            Password = registerViewModel.Password, // TODO: 加密
            Phone = registerViewModel.Phone
        });
        _context.SaveChanges();
        
        var token = GenerateJwt(registerViewModel.Username);
        
        return new JsonResult(new {
            token = token,
            username = registerViewModel.Username
        });
    }
    
    private string GenerateJwt(string username)
    {
        var claims = new[] {
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddMinutes(Convert.ToInt32(_configuration.GetSection("JWT")["Expires"]))).ToUnixTimeSeconds()}"),
            new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT")["IssuerSigningKey"]));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var securityToken = new JwtSecurityToken(
            issuer: _configuration.GetSection("JWT")["ValidIssuer"],
            audience: _configuration.GetSection("JWT")["ValidAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration.GetSection("JWT")["Expires"])),
            signingCredentials: signingCredentials
            );
 
        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
 
        return token;
    }
}