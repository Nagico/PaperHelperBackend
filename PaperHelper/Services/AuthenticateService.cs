using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;
using PaperHelper.Exceptions;
using PaperHelper.Utils;
using PaperHelper.ViewModels;

namespace PaperHelper.Services;

public class AuthenticateService
{
    private readonly IConfiguration _configuration;
    private readonly PaperHelperContext _context;
    private readonly string _salt;
    
    public AuthenticateService(IConfiguration configuration, PaperHelperContext paperHelperContext)
    {
        _configuration = configuration;
        _context = paperHelperContext;
        _salt = _configuration.GetValue<string>("Salt");
    }
    
    public AuthenticateViewModel Login(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            throw new AppError("A0210", "请输入用户名和密码");
        
        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        if (user == null || !user.Password!.Equals(EncryptionUtil.Encrypt(password, _salt)))
            throw new AppError("A0210", "用户名或密码错误");
        
        var token = GenerateJwt(user);
        
        return new AuthenticateViewModel
        {
            Token = token,
            Username = username,
            Id = user.Id,
            Phone = user.Phone,
            Avatar = user.Avatar
        };
    }

    public AuthenticateViewModel Register(string username, string password, string phone)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(phone))
            throw new AppError("A0100", "请输入用户名、密码和手机号");

        if (UsernameCount(username) > 0)
            throw new AppError("A0111", "用户名已存在，请重新输入");

        var newUser = new User
        {
            Username = username,
            Password = EncryptionUtil.Encrypt(password, _salt),
            Phone = phone
        };
        
        _context.Users.Add(newUser);
        _context.SaveChanges();
        
        var token = GenerateJwt(newUser);
        
        return new AuthenticateViewModel
        {
            Token = token,
            Username = username,
            Id = newUser.Id,
            Phone = newUser.Phone,
            Avatar = newUser.Avatar
        };
    }
    
    public int UsernameCount(string username)
    {
        return _context.Users.Count(u => u.Username == username);
    }
    
    private string GenerateJwt(User user)
    {
        var claims = new[] {
            new Claim("UserId", user.Id.ToString()),
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