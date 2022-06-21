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

/// <summary>
/// 认证服务
/// </summary>
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
    
    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <returns>token及用户数据</returns>
    /// <exception cref="AppError">登陆失败</exception>
    public AuthenticateViewModel Login(string username, string password)
    {
        // 参数校验
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            throw new AppError("A0210", "请输入用户名和密码");
        
        // 获取当前用户名对应的用户
        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        
        // 用户不存在或密码错误
        if (user == null || !user.Password!.Equals(EncryptionUtil.Encrypt(password, _salt)))
            throw new AppError("A0210", "用户名或密码错误");
        
        // 生成token
        var token = GenerateJwt(user);
        
        // 修改用户登录时间
        user.LastLogin = DateTime.Now;
        _context.SaveChanges();
        
        // 返回结果
        return new AuthenticateViewModel
        {
            Token = token,
            Username = username,
            Id = user.Id,
            Phone = user.Phone,
            Avatar = user.Avatar
        };
    }
    
    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <param name="phone">手机号</param>
    /// <returns>token及用户信息</returns>
    /// <exception cref="AppError">注册失败</exception>
    public AuthenticateViewModel Register(string username, string password, string phone)
    {
        // 参数校验
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(phone))
            throw new AppError("A0100", "请输入用户名、密码和手机号");
        
        // 用户名已存在
        if (UsernameCount(username) > 0)
            throw new AppError("A0111", "用户名已存在，请重新输入");
        
        // 新用户
        var newUser = new User
        {
            Username = username,
            Password = EncryptionUtil.Encrypt(password, _salt), // 密码加密
            Phone = phone,
            CreateTime = DateTime.Now,
            LastLogin = DateTime.Now
        };
        
        // 添加用户
        _context.Users.Add(newUser);
        _context.SaveChanges();
        
        // 生成token
        var token = GenerateJwt(newUser);
        
        // 返回结果
        return new AuthenticateViewModel
        {
            Token = token,
            Username = username,
            Id = newUser.Id,
            Phone = newUser.Phone,
            Avatar = newUser.Avatar
        };
    }
    
    /// <summary>
    /// 统计用户名数量
    /// </summary>
    /// <param name="username">用户名</param>
    /// <returns>数量</returns>
    public int UsernameCount(string username)
    {
        return _context.Users.Count(u => u.Username == username);
    }
    
    /// <summary>
    /// 生成JWT
    /// </summary>
    /// <param name="user">用户</param>
    /// <returns>token</returns>
    private string GenerateJwt(User user)
    {
        var claims = new[] {
            // 用户id
            new Claim("UserId", user.Id.ToString()), 
            // 过期时间
            new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddMinutes(Convert.ToInt32(_configuration.GetSection("JWT")["Expires"]))).ToUnixTimeSeconds()}"),
            // 签发时间
            new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}")
        };
        
        // 加密
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT")["IssuerSigningKey"]));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        // 生成token对象
        var securityToken = new JwtSecurityToken(
            issuer: _configuration.GetSection("JWT")["ValidIssuer"],
            audience: _configuration.GetSection("JWT")["ValidAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration.GetSection("JWT")["Expires"])),
            signingCredentials: signingCredentials
        );
        
        // 获取token
        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
 
        return token;
    }
}