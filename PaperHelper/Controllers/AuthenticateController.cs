using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PaperHelper.Dtos;

namespace PaperHelper.Controllers;

[ApiController]
[Route("api/tokens/")]
public class AuthenticateController : ControllerBase
{
    private readonly IConfiguration _configuration;
    
    public AuthenticateController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public JsonResult Login([FromBody] LoginDto loginDto)
    {
        //User Authentication
        if (string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
        {
            return new JsonResult(new {
                message = "Username or Password is empty"
            });
        }

        //Generate Token
        var token = GenerateJwt(loginDto.Username);
        
        return new JsonResult(new {
            token = token,
            username = loginDto.Username
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