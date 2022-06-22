using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;
using PaperHelper.Exceptions;
using PaperHelper.Services.Serializers;
using PaperHelper.Utils;

namespace PaperHelper.Services;

/// <summary>
/// 用户服务
/// </summary>
public class UserService
{
    private readonly IConfiguration _configuration;
    private readonly PaperHelperContext _context;
    private readonly UserSerializer _userSerializer;
    private readonly ProjectSerializer _projectSerializer;
    private readonly string _salt;
    
    public UserService(IConfiguration configuration, PaperHelperContext paperHelperContext)
    {
        _configuration = configuration;
        _context = paperHelperContext;
        _userSerializer = new UserSerializer(_context);
        _projectSerializer = new ProjectSerializer(_context);
        _salt = _configuration.GetValue<string>("Salt");
    }
    
    /// <summary>
    /// 获取用户对象
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>用户对象</returns>
    /// <exception cref="AppError">用户不存在</exception>
    private User GetUser(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            throw new AppError("A0514");
        }
        return user;
    }
    
    /// <summary>
    /// 获取用户简要信息
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>JSON对象</returns>
    public JObject GetUserPartDetail(int id)
    {
        var user = GetUser(id);
        
        return _userSerializer.UserInfo(user);
    }
    
    /// <summary>
    /// 获取用户全部详细信息
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>JSON对象</returns>
    public JObject GetUserFullDetail(int id)
    {
        var user = GetUser(id);
        
        return _userSerializer.UserDetail(user);
    }
    
    /// <summary>
    /// 更新用户信息
    /// </summary>
    /// <param name="id">待更新用户ID</param>
    /// <param name="username">用户名</param>
    /// <param name="oldPassword">原始密码</param>
    /// <param name="newPassword">新密码</param>
    /// <param name="phone">手机号</param>
    /// <returns>用户详情</returns>
    /// <exception cref="AppError">更新失败</exception>
    public JObject UpdateUser(int id, string? username, string? oldPassword, string? newPassword, string? phone, int loginUserId)
    {
        if (loginUserId != id)
        {
            throw new AppError("A0312");
        }
        
        var user = GetUser(id);
        
        // 更新用户名
        if (username != null)
        {
            if (_context.Users.Any(u => u.Username == username && u.Id != id))
            {
                throw new AppError("A0111", "用户名已存在，请重新输入");
            }
            user.Username = username;
        }

        // 更新密码
        if (oldPassword != null || newPassword != null)
        {
            if (oldPassword == null || newPassword == null)
            {
                throw new AppError("A0420", "请输入旧密码和新密码");
            }
            if (user.Password != EncryptionUtil.Encrypt(oldPassword, _salt))
            {
                throw new AppError("A0210", "旧密码错误，请重新输入");
            }
            user.Password = EncryptionUtil.Encrypt(newPassword, _salt);
        }
        
        // 更新手机号
        if (phone != null)
        {
            user.Phone = phone;
        }
        
        _context.SaveChanges();
        
        return _userSerializer.UserDetail(user);
    }
    
    /// <summary>
    /// 更新用户头像
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <param name="file">头像文件</param>
    /// <param name="loginUserId">当前登录用户ID</param>
    /// <returns>用户详情</returns>
    /// <exception cref="AppError">更新失败</exception>
    public JObject UpdateUserAvatar(int id, IFormFile file, int loginUserId)
    {
        if (loginUserId != id)
        {
            throw new AppError("A0312");
        }
        
        var user = GetUser(id);
        
        var ali = new AliYunOssUtil(_configuration);
        
        // 上传头像
        var avatarUrl = ali.UploadFile("avatar", file);

        // 删除旧头像
        if (Path.GetFileName(user.Avatar.LocalPath) != "default.jpg")
        {
            ali.DeleteFile("avatar", user.Avatar.LocalPath);
        }

        // 修改头像
        user.Avatar = avatarUrl;

        _context.SaveChanges();
        
        return _userSerializer.UserDetail(user);
    }
    
    /// <summary>
    /// 加入项目
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <param name="invitationCode">邀请码</param>
    /// <returns>项目详情</returns>
    /// <exception cref="AppError">加入失败</exception>
    public JObject JoinProject(int id, string invitationCode)
    {
        // 查找邀请码对应的项目
        var project = _context.Projects.Include("Members").FirstOrDefault(x => x.InvitationCode == invitationCode);
        
        if (project == null)
        {
            throw new AppError("A0430", "邀请码不存在");
        }
        
        // 查找用户
        var user = _context.Users.Find(id);
        if (user == null)
        {
            throw new AppError("A0430", "用户不存在");
        }
        
        // 判断用户是否已经加入项目
        if (project.Members.Any(x => x.UserId == id))
        {
            throw new AppError("A0430", "用户重复加入");
        }
        
        // 添加用户到项目
        project.Members.Add(new UserProject
        {
            User = user,
            Project = project,
            IsOwner = false,
            AccessTime = DateTime.Now,
            CreateTime = DateTime.Now
        });
        project.UpdateTime = DateTime.Now;
        
        // 保存
        _context.SaveChanges();
        return _projectSerializer.ProjectDetail(project);
    }
}